using Crosscutting.Enum;

namespace Crosscutting.DTO.Discount
{
    public class ApplyDiscountRequestDTO
    {
        public DiscountScopeEnum Scope { get; set; }
        public int? ProposalId { get; set; }
        public int? SaleId { get; set; }
        public int? ProposalItemId { get; set; }
        public int? SaleItemId { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
