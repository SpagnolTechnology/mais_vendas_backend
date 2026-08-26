using AppService.AutoMapping.Base;
using Crosscutting.DTO.DiscountRule;
using Domain.Entity;

namespace AppService.AutoMapping.DiscountRule
{
    public class DiscountRuleProfile : BaseProfile
    {
        public DiscountRuleProfile()
        {
            CreateMap<CreateDiscountRuleRequestDTO, DiscountRuleEntity>();
            CreateMap<DiscountRuleEntity, DiscountRuleResponseDTO>();
        }
    }
}
