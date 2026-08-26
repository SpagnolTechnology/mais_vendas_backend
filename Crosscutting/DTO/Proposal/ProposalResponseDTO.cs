using Crosscutting.DTO.Base;
using Crosscutting.Enum;

namespace Crosscutting.DTO.Proposal
{
    public class ProposalResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string ExternalClientId { get; set; } = string.Empty;
        public string SellerEmail { get; set; } = string.Empty;
        public int PaymentConditionId { get; set; }
        public SaleTypeEnum SaleType { get; set; }
        public ProposalStatusEnum Status { get; set; }
        public DateTime? ValidUntil { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool HasStockWarning { get; set; }
        public string? Notes { get; set; }
        public List<ProposalItemResponseDTO> Items { get; set; } = new();
    }
}
