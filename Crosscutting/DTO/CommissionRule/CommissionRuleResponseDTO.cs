using Crosscutting.DTO.Base;
using Crosscutting.Enum;

namespace Crosscutting.DTO.CommissionRule
{
    public class CommissionRuleResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public CommissionCalculationScopeEnum? CalculationScope { get; set; }
        public decimal CommissionPercent { get; set; }
        public bool IsActive { get; set; }
    }
}
