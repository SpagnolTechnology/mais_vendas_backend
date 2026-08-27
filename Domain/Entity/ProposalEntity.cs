using Crosscutting.Enum;
using Domain.Base;

namespace Domain.Entity
{
    public class ProposalEntity : BaseEntity
    {
        public int Id { get; set; }
        public Guid ProposalUuid { get; set; }
        public string Number { get; set; } = string.Empty;
        public string ExternalClientId { get; set; } = string.Empty;
        public string SellerEmail { get; set; } = string.Empty;
        public int PaymentConditionId { get; set; }
        public SaleTypeEnum SaleType { get; set; }
        public ProposalStatusEnum Status { get; set; } = ProposalStatusEnum.Draft;
        public DateTime? ValidUntil { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool HasStockWarning { get; set; }
        public string? Notes { get; set; }

        public PaymentConditionEntity? PaymentCondition { get; set; }
        public ICollection<ProposalItemEntity> Items { get; set; } = new List<ProposalItemEntity>();
    }
}
