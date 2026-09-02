using System.Globalization;
using System.Xml.Linq;
using Crosscutting.CustomException;
using Crosscutting.DTO.NfeImport;
using Crosscutting.Helpers;

namespace Crosscutting.Invoice
{
    public class NfeInvoiceXmlParser : IInvoiceXmlParser
    {
        public string DocumentType => "NFe";

        public bool CanParse(XDocument document)
        {
            return FindDescendant(document.Root, "infNFe") is not null;
        }

        public ParsedInvoiceDTO Parse(XDocument document)
        {
            XElement? infNFe = FindDescendant(document.Root, "infNFe");
            if (infNFe is null)
                throw new CustomBusinessException("Ops... O XML não contém uma NFe válida.");

            XElement? ide = FindChild(infNFe, "ide");
            XElement? emit = FindChild(infNFe, "emit");
            if (ide is null || emit is null)
                throw new CustomBusinessException("Ops... O XML da NFe está incompleto (ide/emit).");

            string invoiceNumber = GetChildValue(ide, "nNF") ?? string.Empty;
            string invoiceSeries = GetChildValue(ide, "serie") ?? string.Empty;
            string? invoiceKey = ExtractInvoiceKey(document, infNFe);
            DateTime entryDate = ParseEntryDate(GetChildValue(ide, "dhEmi"), GetChildValue(ide, "dEmi"));

            ParsedInvoiceSupplierDTO supplier = ParseSupplier(emit);

            List<ParsedInvoiceItemDTO> items = infNFe.Elements()
                .Where(e => e.Name.LocalName == "det")
                .Select((det, index) => ParseItem(det, index + 1))
                .Where(item => item is not null)
                .Cast<ParsedInvoiceItemDTO>()
                .ToList();

            if (!items.Any())
                throw new CustomBusinessException("Ops... A NFe não contém itens para importação.");

            return new ParsedInvoiceDTO
            {
                InvoiceNumber = invoiceNumber,
                InvoiceSeries = invoiceSeries,
                InvoiceKey = invoiceKey,
                EntryDate = entryDate,
                Supplier = supplier,
                Items = items
            };
        }

        private static ParsedInvoiceSupplierDTO ParseSupplier(XElement emit)
        {
            XElement? address = FindChild(emit, "enderEmit");
            string? street = address is not null ? GetChildValue(address, "xLgr") : null;
            string? number = address is not null ? GetChildValue(address, "nro") : null;
            string? district = address is not null ? GetChildValue(address, "xBairro") : null;
            string? city = address is not null ? GetChildValue(address, "xMun") : null;
            string? state = address is not null ? GetChildValue(address, "UF") : null;
            string? zipCode = address is not null ? GetChildValue(address, "CEP") : null;

            string? fullAddress = string.Join(", ",
                new[] { street, number, district }.Where(part => !string.IsNullOrWhiteSpace(part)));

            string document = DocumentHelper.NormalizeDigits(
                GetChildValue(emit, "CNPJ") ?? GetChildValue(emit, "CPF"));

            return new ParsedInvoiceSupplierDTO
            {
                Name = GetChildValue(emit, "xNome") ?? string.Empty,
                Document = document,
                Address = string.IsNullOrWhiteSpace(fullAddress) ? null : fullAddress,
                City = city,
                State = state,
                ZipCode = DocumentHelper.NormalizeDigits(zipCode)
            };
        }

        private static ParsedInvoiceItemDTO? ParseItem(XElement det, int fallbackLineNumber)
        {
            XElement? prod = FindDescendant(det, "prod");
            if (prod is null)
                return null;

            string? lineNumberValue = det.Attribute("nItem")?.Value;
            int lineNumber = int.TryParse(lineNumberValue, out int parsedLine)
                ? parsedLine
                : fallbackLineNumber;

            decimal quantity = ParseDecimal(GetChildValue(prod, "qCom"));
            decimal unitCost = ParseDecimal(GetChildValue(prod, "vUnCom"));

            if (quantity <= 0)
                return null;

            return new ParsedInvoiceItemDTO
            {
                LineNumber = lineNumber,
                ProductCode = GetChildValue(prod, "cProd") ?? string.Empty,
                Ean = GetChildValue(prod, "cEAN"),
                ProductName = GetChildValue(prod, "xProd") ?? string.Empty,
                Quantity = quantity,
                UnitCost = unitCost
            };
        }

        private static string? ExtractInvoiceKey(XDocument document, XElement infNFe)
        {
            string? chNFe = FindDescendant(document.Root, "chNFe")?.Value;
            if (!string.IsNullOrWhiteSpace(chNFe))
                return DocumentHelper.NormalizeDigits(chNFe);

            string? idAttribute = infNFe.Attribute("Id")?.Value;
            if (string.IsNullOrWhiteSpace(idAttribute))
                return null;

            string normalized = DocumentHelper.NormalizeDigits(idAttribute);
            return normalized.Length == 44 ? normalized : null;
        }

        private static DateTime ParseEntryDate(string? dateTimeValue, string? dateValue)
        {
            if (!string.IsNullOrWhiteSpace(dateTimeValue)
                && DateTime.TryParse(dateTimeValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime parsedDateTime))
            {
                return parsedDateTime;
            }

            if (!string.IsNullOrWhiteSpace(dateTimeValue)
                && DateTime.TryParse(dateTimeValue, out DateTime parsedLocalDateTime))
            {
                return parsedLocalDateTime;
            }

            if (!string.IsNullOrWhiteSpace(dateValue)
                && DateTime.TryParseExact(dateValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }

            return DateTime.UtcNow;
        }

        private static decimal ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal parsed))
                return parsed;

            return decimal.TryParse(value, out parsed) ? parsed : 0m;
        }

        private static XElement? FindDescendant(XElement? root, string localName)
        {
            if (root is null)
                return null;

            return root.Descendants().FirstOrDefault(e => e.Name.LocalName == localName);
        }

        private static XElement? FindChild(XElement parent, string localName)
        {
            return parent.Elements().FirstOrDefault(e => e.Name.LocalName == localName);
        }

        private static string? GetChildValue(XElement parent, string localName)
        {
            return FindChild(parent, localName)?.Value;
        }
    }
}
