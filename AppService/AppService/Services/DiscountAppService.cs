using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.CustomException;
using Crosscutting.DTO.Discount;
using Crosscutting.Enum;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class DiscountAppService : BaseService<IDiscountRepository, DiscountEntity>, IDiscountAppService
    {
        private readonly IDiscountRuleRepository _discountRuleRepository;

        public DiscountAppService(
            IMapper mapper,
            IDiscountRepository repository,
            IDiscountRuleRepository discountRuleRepository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
            _discountRuleRepository = discountRuleRepository;
        }

        public async Task<DiscountResponseDTO> ApplyDiscountAsync(ApplyDiscountRequestDTO request, CancellationToken ct = default)
        {
            IReadOnlyList<DiscountRuleEntity> rules = await _discountRuleRepository.GetActiveRulesForUserAsync(_role, _email, ct);

            if (!rules.Any())
                throw new CustomBusinessException("Ops... Nenhuma regra de desconto ativa encontrada para o usuário.");

            decimal maxDiscountPercent = rules.Min(r => r.MaxDiscountPercent);

            if (request.DiscountPercent > maxDiscountPercent)
                throw new CustomBusinessException("Ops... O percentual de desconto excede o limite permitido.");

            List<DiscountRuleEntity> rulesWithAmountLimit = rules.Where(r => r.MaxDiscountAmount.HasValue).ToList();
            if (rulesWithAmountLimit.Any())
            {
                decimal maxDiscountAmount = rulesWithAmountLimit.Min(r => r.MaxDiscountAmount!.Value);
                if (request.DiscountAmount > maxDiscountAmount)
                    throw new CustomBusinessException("Ops... O valor de desconto excede o limite permitido.");
            }

            decimal appliedAmount = request.DiscountAmount > 0
                ? request.DiscountAmount
                : request.DiscountPercent;

            DateTime now = GetCurrentDateTime();

            DiscountEntity entity = new()
            {
                Scope = request.Scope,
                ProposalId = request.ProposalId,
                SaleId = request.SaleId,
                ProposalItemId = request.ProposalItemId,
                SaleItemId = request.SaleItemId,
                DiscountPercent = request.DiscountPercent,
                DiscountAmount = request.DiscountAmount,
                AppliedAmount = appliedAmount,
                AuthorizedBy = _email,
                AuthorizedRole = _role,
                CreatedAt = now,
                CreatedBy = _email
            };

            DiscountEntity createdEntity = await AddAsync(entity, ct);
            return _mapper.Map<DiscountResponseDTO>(createdEntity);
        }

        public async Task<IReadOnlyList<DiscountResponseDTO>> GetByProposalIdAsync(int proposalId, CancellationToken ct = default)
        {
            IReadOnlyList<DiscountEntity> entities = await _repository.GetByProposalIdAsync(proposalId, ct);
            return _mapper.Map<IReadOnlyList<DiscountResponseDTO>>(entities);
        }

        public async Task<IReadOnlyList<DiscountResponseDTO>> GetBySaleIdAsync(int saleId, CancellationToken ct = default)
        {
            IReadOnlyList<DiscountEntity> entities = await _repository.GetBySaleIdAsync(saleId, ct);
            return _mapper.Map<IReadOnlyList<DiscountResponseDTO>>(entities);
        }

        private DateTime GetCurrentDateTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
        }
    }
}
