namespace Crosscutting.DTO.DiscountRule
{
    public class UpdateDiscountRuleRequestDTO
    {
        public string? Role { get; set; }
        public string? UserEmail { get; set; }
        public decimal MaxDiscountPercent { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
