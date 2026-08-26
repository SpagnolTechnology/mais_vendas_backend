using Domain.Base;

namespace Domain.Entity
{
    public class StockAdjustmentEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime AdjustmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public bool IsConfirmed { get; set; }

        public ICollection<StockAdjustmentItemEntity> Items { get; set; } = new List<StockAdjustmentItemEntity>();
    }
}
