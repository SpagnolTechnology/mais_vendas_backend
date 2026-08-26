using Crosscutting.DTO.Base;

namespace Crosscutting.DTO.Proposal
{
    public class ProposalItemResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LineSubtotal { get; set; }
        public decimal IcmsPercent { get; set; }
        public decimal IcmsAmount { get; set; }
        public decimal IssPercent { get; set; }
        public decimal IssAmount { get; set; }
        public decimal PisPercent { get; set; }
        public decimal PisAmount { get; set; }
        public decimal CofinsPercent { get; set; }
        public decimal CofinsAmount { get; set; }
        public decimal LineTotal { get; set; }
        public bool StockUnavailable { get; set; }
    }
}
