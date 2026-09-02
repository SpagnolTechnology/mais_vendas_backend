using System.Xml.Linq;
using AppService.AppService.Interfaces;
using Crosscutting.CustomException;
using Crosscutting.DTO.NfeImport;
using Crosscutting.DTO.ProductPurchaseEntry;
using Crosscutting.Enum;
using Crosscutting.Helpers;
using Crosscutting.Invoice;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;

namespace AppService.AppService.Services
{
    public class NfeImportAppService : INfeImportAppService
    {
        private readonly InvoiceXmlParserResolver _parserResolver;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductPurchaseEntryRepository _productPurchaseEntryRepository;
        private readonly IProductPurchaseEntryAppService _productPurchaseEntryAppService;

        public NfeImportAppService(
            InvoiceXmlParserResolver parserResolver,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository,
            IProductPurchaseEntryRepository productPurchaseEntryRepository,
            IProductPurchaseEntryAppService productPurchaseEntryAppService)
        {
            _parserResolver = parserResolver;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _productPurchaseEntryRepository = productPurchaseEntryRepository;
            _productPurchaseEntryAppService = productPurchaseEntryAppService;
        }

        public async Task<NfeImportPreviewResponseDTO> PreviewAsync(string xmlContent, CancellationToken ct = default)
        {
            ParsedInvoiceDTO parsed = ParseInvoice(xmlContent);

            bool isDuplicate = await IsDuplicateInvoiceKeyAsync(parsed.InvoiceKey, ct);
            NfeImportSupplierMatchDTO supplierMatch = await BuildSupplierMatchAsync(parsed.Supplier, ct);
            List<NfeImportItemPreviewDTO> items = await BuildItemPreviewsAsync(parsed.Items, ct);
            List<string> warnings = BuildWarnings(supplierMatch, isDuplicate);

            return new NfeImportPreviewResponseDTO
            {
                InvoiceNumber = parsed.InvoiceNumber,
                InvoiceSeries = parsed.InvoiceSeries,
                InvoiceKey = parsed.InvoiceKey,
                EntryDate = parsed.EntryDate,
                SupplierMatch = supplierMatch,
                Items = items,
                Warnings = warnings,
                IsDuplicateInvoiceKey = isDuplicate
            };
        }

        public async Task<ProductPurchaseEntryResponseDTO> ConfirmImportAsync(
            NfeImportConfirmRequestDTO request,
            CancellationToken ct = default)
        {
            if (request.SupplierId <= 0)
                throw new CustomBusinessException("Ops... O fornecedor é obrigatório para importar a NFe.");

            if (!request.Items.Any())
                throw new CustomBusinessException("Ops... A entrada de compra deve conter ao menos um item.");

            if (await IsDuplicateInvoiceKeyAsync(request.InvoiceKey, ct))
                throw new CustomBusinessException("Ops... Já existe uma entrada com esta chave NFe.");

            CreateProductPurchaseEntryRequestDTO createRequest = new()
            {
                SupplierId = request.SupplierId,
                InvoiceNumber = request.InvoiceNumber,
                InvoiceSeries = request.InvoiceSeries,
                InvoiceKey = request.InvoiceKey,
                EntryDate = request.EntryDate,
                Notes = request.Notes ?? "Importado via XML",
                Items = request.Items.Select(item => new CreateProductPurchaseEntryItemRequestDTO
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitCost,
                    MarkupPercent = item.MarkupPercent
                }).ToList()
            };

            return await _productPurchaseEntryAppService.CreateAndConfirmAsync(createRequest, ct);
        }

        private ParsedInvoiceDTO ParseInvoice(string xmlContent)
        {
            if (string.IsNullOrWhiteSpace(xmlContent))
                throw new CustomBusinessException("Ops... O conteúdo XML é obrigatório.");

            try
            {
                XDocument document = XDocument.Parse(xmlContent, LoadOptions.None);
                IInvoiceXmlParser parser = _parserResolver.Resolve(document);
                return parser.Parse(document);
            }
            catch (CustomBusinessException)
            {
                throw;
            }
            catch (Exception ex) when (ex is System.Xml.XmlException or FormatException)
            {
                throw new CustomBusinessException("Ops... O XML informado não é válido.");
            }
        }

        private async Task<bool> IsDuplicateInvoiceKeyAsync(string? invoiceKey, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(invoiceKey))
                return false;

            return await _productPurchaseEntryRepository.ExistsByInvoiceKeyAsync(invoiceKey, ct);
        }

        private async Task<NfeImportSupplierMatchDTO> BuildSupplierMatchAsync(
            ParsedInvoiceSupplierDTO supplier,
            CancellationToken ct)
        {
            SupplierEntity? matchedSupplier = await _supplierRepository.GetByDocumentAsync(supplier.Document, ct);

            NfeSuggestedSupplierDTO? suggestedSupplier = null;
            if (matchedSupplier is null && !string.IsNullOrWhiteSpace(supplier.Document))
            {
                suggestedSupplier = new NfeSuggestedSupplierDTO
                {
                    Name = supplier.Name,
                    Document = supplier.Document,
                    Address = supplier.Address,
                    City = supplier.City,
                    State = supplier.State,
                    ZipCode = supplier.ZipCode
                };
            }

            return new NfeImportSupplierMatchDTO
            {
                MatchedSupplierId = matchedSupplier?.Id,
                SuggestedSupplier = suggestedSupplier
            };
        }

        private async Task<List<NfeImportItemPreviewDTO>> BuildItemPreviewsAsync(
            List<ParsedInvoiceItemDTO> items,
            CancellationToken ct)
        {
            List<NfeImportItemPreviewDTO> previews = new();

            foreach (ParsedInvoiceItemDTO item in items)
            {
                ProductMatchResult matchResult = await MatchProductAsync(item, ct);

                previews.Add(new NfeImportItemPreviewDTO
                {
                    LineNumber = item.LineNumber,
                    XmlProductCode = item.ProductCode,
                    XmlEan = item.Ean,
                    XmlProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitCost,
                    MatchStatus = matchResult.MatchStatus,
                    MatchedProductId = matchResult.Product?.Id,
                    MatchedProductName = matchResult.Product?.Name,
                    DefaultMarkupPercent = matchResult.Product?.MarkupPercent
                });
            }

            return previews;
        }

        private async Task<ProductMatchResult> MatchProductAsync(ParsedInvoiceItemDTO item, CancellationToken ct)
        {
            ProductEntity? product = null;
            NfeImportItemMatchStatusEnum matchStatus = NfeImportItemMatchStatusEnum.Unmatched;

            if (!string.IsNullOrWhiteSpace(item.ProductCode))
            {
                product = await _productRepository.GetBySkuAsync(item.ProductCode, ct);
                if (product is not null)
                    matchStatus = NfeImportItemMatchStatusEnum.MatchedBySku;
            }

            if (product is null && DocumentHelper.IsValidNfeEan(item.Ean))
            {
                product = await _productRepository.GetByEanAsync(item.Ean!, ct);
                if (product is not null)
                    matchStatus = NfeImportItemMatchStatusEnum.MatchedByEan;
            }

            return new ProductMatchResult(product, matchStatus);
        }

        private static List<string> BuildWarnings(NfeImportSupplierMatchDTO supplierMatch, bool isDuplicate)
        {
            List<string> warnings = new();

            if (supplierMatch.MatchedSupplierId is null && supplierMatch.SuggestedSupplier is not null)
                warnings.Add("Fornecedor não cadastrado. Selecione ou cadastre antes de importar.");

            if (isDuplicate)
                warnings.Add("Já existe uma entrada com esta chave NFe.");

            return warnings;
        }

        private sealed record ProductMatchResult(
            ProductEntity? Product,
            NfeImportItemMatchStatusEnum MatchStatus);
    }
}
