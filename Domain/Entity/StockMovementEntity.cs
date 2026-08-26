using Crosscutting.Enum;
using Domain.Base;

namespace Domain.Entity
{
    public class StockMovementEntity : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public StockMovementTypeEnum MovementType { get; set; }
        public decimal Quantity { get; set; }
        public decimal BalanceAfter { get; set; }
        public int? ProductPurchaseEntryItemId { get; set; }
        public int? StockAdjustmentItemId { get; set; }
        public int? SaleId { get; set; }
        public string? OriginDocumentNumber { get; set; }
        public string? Notes { get; set; }

        public ProductEntity? Product { get; set; }
    }
}
