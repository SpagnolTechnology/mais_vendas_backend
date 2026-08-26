using AppService.AutoMapping.Base;
using Crosscutting.DTO.CommissionRule;
using Domain.Entity;

namespace AppService.AutoMapping.CommissionRule
{
    public class CommissionRuleProfile : BaseProfile
    {
        public CommissionRuleProfile()
        {
            CreateMap<CreateCommissionRuleRequestDTO, CommissionRuleEntity>();
            CreateMap<CommissionRuleEntity, CommissionRuleResponseDTO>();
        }
    }
}
