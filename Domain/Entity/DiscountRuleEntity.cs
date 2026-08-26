using Domain.Base;

namespace Domain.Entity
{
    public class DiscountRuleEntity : BaseEntity
    {
        public int Id { get; set; }
        public string? Role { get; set; }
        public string? UserEmail { get; set; }
        public decimal MaxDiscountPercent { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
