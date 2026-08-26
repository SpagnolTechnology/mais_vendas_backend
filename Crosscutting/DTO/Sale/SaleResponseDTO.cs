using Crosscutting.DTO.Base;
using Crosscutting.Enum;

namespace Crosscutting.DTO.Sale
{
    public class SaleResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public int? ProposalId { get; set; }
        public string ExternalClientId { get; set; } = string.Empty;
        public string? ExternalBillingId { get; set; }
        public string SellerEmail { get; set; } = string.Empty;
        public int PaymentConditionId { get; set; }
        public SaleTypeEnum SaleType { get; set; }
        public SaleStatusEnum Status { get; set; }
        public DateTime? SoldAt { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleItemResponseDTO> Items { get; set; } = new();
    }
}
