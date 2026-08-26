using Crosscutting.External;
using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.CustomException;
using Crosscutting.DTO.Sale;
using Crosscutting.Enum;
using Crosscutting.Helpers;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class SaleAppService : BaseService<ISaleRepository, SaleEntity>, ISaleAppService
    {
        private readonly ISalesOperationsRepository _salesOperationsRepository;
        private readonly IDocumentSequenceRepository _documentSequenceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IExternalClientService _externalClientService;

        public SaleAppService(
            IMapper mapper,
            ISaleRepository repository,
            ISalesOperationsRepository salesOperationsRepository,
            IDocumentSequenceRepository documentSequenceRepository,
            IProductRepository productRepository,
            IExternalClientService externalClientService,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
            _salesOperationsRepository = salesOperationsRepository;
            _documentSequenceRepository = documentSequenceRepository;
            _productRepository = productRepository;
            _externalClientService = externalClientService;
        }

        public async Task<SaleResponseDTO> GetResponseByIdAsync(int id, CancellationToken ct = default)
        {
            SaleEntity entity = await GetSaleWithDetailsAsync(id, ct);
            return _mapper.Map<SaleResponseDTO>(entity);
        }

        public async Task<SaleResponseDTO> CreateAsync(CreateSaleRequestDTO request, CancellationToken ct = default)
        {
            if (!request.Items.Any())
                throw new CustomBusinessException("Ops... A venda deve conter ao menos um item.");

            await _externalClientService.ValidateClientAsync(request.ExternalClientId, ct);

            DateTime now = GetCurrentDateTime();
            string userEmail = GetCurrentUserEmail();
            int year = now.Year;

            SaleEntity entity = new()
            {
                ProposalId = request.ProposalId,
                ExternalClientId = request.ExternalClientId,
                SellerEmail = userEmail,
                PaymentConditionId = request.PaymentConditionId,
                SaleType = request.SaleType,
                Status = SaleStatusEnum.Pending,
                Number = await _documentSequenceRepository.GetNextNumberAsync(DocumentTypeEnum.Sale, year, ct),
                CreatedAt = now,
                CreatedBy = userEmail
            };

            foreach (CreateSaleItemRequestDTO itemRequest in request.Items)
            {
                SaleItemEntity item = await BuildSaleItemAsync(itemRequest, now, userEmail, ct);
                entity.Items.Add(item);
            }

            RecalculateSaleTotals(entity);

            SaleEntity createdEntity = await AddAsync(entity, ct);
            SaleEntity entityWithDetails = await GetSaleWithDetailsAsync(createdEntity.Id, ct);
            return _mapper.Map<SaleResponseDTO>(entityWithDetails);
        }

        public async Task<SaleResponseDTO> ConfirmAsync(int id, CancellationToken ct = default)
        {
            SaleEntity entity = await GetSaleWithDetailsAsync(id, ct);

            if (entity.Status == SaleStatusEnum.Confirmed)
                throw new CustomBusinessException("Ops... A venda já está confirmada.");

            DateTime now = GetCurrentDateTime();
            await _salesOperationsRepository.ConfirmSaleAsync(id, GetCurrentUserEmail(), now, ct);

            SaleEntity confirmedEntity = await GetSaleWithDetailsAsync(id, ct);
            return _mapper.Map<SaleResponseDTO>(confirmedEntity);
        }

        private async Task<SaleItemEntity> BuildSaleItemAsync(
            CreateSaleItemRequestDTO itemRequest,
            DateTime now,
            string userEmail,
            CancellationToken ct)
        {
            ProductEntity product = await _productRepository.GetByIdAsync(itemRequest.ProductId)
                ?? throw new CustomBusinessException("Ops... O registro buscado não foi encontrado.");

            decimal lineSubtotal = SalesCalculationHelper.CalculateLineSubtotal(
                itemRequest.Quantity,
                itemRequest.UnitPrice,
                itemRequest.DiscountPercent,
                itemRequest.DiscountAmount);

            decimal icmsAmount = SalesCalculationHelper.CalculateTaxAmount(lineSubtotal, itemRequest.IcmsPercent);
            decimal issAmount = SalesCalculationHelper.CalculateTaxAmount(lineSubtotal, itemRequest.IssPercent);
            decimal pisAmount = SalesCalculationHelper.CalculateTaxAmount(lineSubtotal, itemRequest.PisPercent);
            decimal cofinsAmount = SalesCalculationHelper.CalculateTaxAmount(lineSubtotal, itemRequest.CofinsPercent);

            return new SaleItemEntity
            {
                ProductId = itemRequest.ProductId,
                Quantity = itemRequest.Quantity,
                UnitPrice = itemRequest.UnitPrice,
                DiscountPercent = itemRequest.DiscountPercent,
                DiscountAmount = itemRequest.DiscountAmount,
                LineSubtotal = lineSubtotal,
                IcmsPercent = itemRequest.IcmsPercent,
                IcmsAmount = icmsAmount,
                IssPercent = itemRequest.IssPercent,
                IssAmount = issAmount,
                PisPercent = itemRequest.PisPercent,
                PisAmount = pisAmount,
                CofinsPercent = itemRequest.CofinsPercent,
                CofinsAmount = cofinsAmount,
                LineTotal = SalesCalculationHelper.CalculateLineTotal(lineSubtotal, icmsAmount, issAmount, pisAmount, cofinsAmount),
                CreatedAt = now,
                CreatedBy = userEmail
            };
        }

        private static void RecalculateSaleTotals(SaleEntity entity)
        {
            entity.SubtotalAmount = entity.Items.Sum(i => i.LineSubtotal);
            entity.DiscountAmount = entity.Items.Sum(i => i.DiscountAmount);
            entity.TaxAmount = entity.Items.Sum(i => i.IcmsAmount + i.IssAmount + i.PisAmount + i.CofinsAmount);
            entity.TotalAmount = entity.Items.Sum(i => i.LineTotal);
        }

        private async Task<SaleEntity> GetSaleWithDetailsAsync(int id, CancellationToken ct)
        {
            return await _repository.GetByIdWithDetailsAsync(id, ct)
                ?? throw new CustomBusinessException("Ops... O registro buscado não foi encontrado.");
        }

        private DateTime GetCurrentDateTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
        }

        private string GetCurrentUserEmail()
        {
            return _email;
        }
    }
}
