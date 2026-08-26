using Crosscutting.Enum;
using Domain.Base;

namespace Domain.Entity
{
    public class DiscountEntity : BaseEntity
    {
        public int Id { get; set; }
        public DiscountScopeEnum Scope { get; set; }
        public int? ProposalId { get; set; }
        public int? SaleId { get; set; }
        public int? ProposalItemId { get; set; }
        public int? SaleItemId { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AppliedAmount { get; set; }
        public string AuthorizedBy { get; set; } = string.Empty;
        public string AuthorizedRole { get; set; } = string.Empty;
    }
}
