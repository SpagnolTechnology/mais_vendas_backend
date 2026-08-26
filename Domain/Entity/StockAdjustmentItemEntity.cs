using Domain.Base;

namespace Domain.Entity
{
    public class StockAdjustmentItemEntity : BaseEntity
    {
        public int Id { get; set; }
        public int StockAdjustmentId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string? Notes { get; set; }

        public StockAdjustmentEntity? StockAdjustment { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
