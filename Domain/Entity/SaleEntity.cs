using Crosscutting.Enum;
using Domain.Base;

namespace Domain.Entity
{
    public class SaleEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public int? ProposalId { get; set; }
        public string ExternalClientId { get; set; } = string.Empty;
        public string? ExternalBillingId { get; set; }
        public string SellerEmail { get; set; } = string.Empty;
        public int PaymentConditionId { get; set; }
        public SaleTypeEnum SaleType { get; set; }
        public SaleStatusEnum Status { get; set; } = SaleStatusEnum.Pending;
        public DateTime? SoldAt { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public ProposalEntity? Proposal { get; set; }
        public PaymentConditionEntity? PaymentCondition { get; set; }
        public ICollection<SaleItemEntity> Items { get; set; } = new List<SaleItemEntity>();
        public ICollection<CommissionEntity> Commissions { get; set; } = new List<CommissionEntity>();
    }
}
