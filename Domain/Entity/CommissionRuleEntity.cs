using Crosscutting.Enum;
using Domain.Base;

namespace Domain.Entity
{
    public class CommissionRuleEntity : BaseEntity
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public CommissionCalculationScopeEnum? CalculationScope { get; set; }
        public decimal CommissionPercent { get; set; }
        public bool IsActive { get; set; } = true;

        public ProductEntity? Product { get; set; }
    }
}
