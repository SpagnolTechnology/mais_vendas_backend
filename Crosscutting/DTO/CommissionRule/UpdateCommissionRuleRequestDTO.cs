using Crosscutting.Enum;

namespace Crosscutting.DTO.CommissionRule
{
    public class UpdateCommissionRuleRequestDTO
    {
        public int? ProductId { get; set; }
        public CommissionCalculationScopeEnum? CalculationScope { get; set; }
        public decimal CommissionPercent { get; set; }
        public bool IsActive { get; set; }
    }
}
