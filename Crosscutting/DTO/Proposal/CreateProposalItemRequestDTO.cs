namespace Crosscutting.DTO.Proposal
{
    public class CreateProposalItemRequestDTO
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal? IcmsPercent { get; set; }
        public decimal? IssPercent { get; set; }
        public decimal? PisPercent { get; set; }
        public decimal? CofinsPercent { get; set; }
    }
}
