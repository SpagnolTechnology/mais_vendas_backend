using Domain.Base;

namespace Domain.Entity
{
    public class ProductPurchaseEntryItemEntity : BaseEntity
    {
        public int Id { get; set; }
        public int ProductPurchaseEntryId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal MarkupPercent { get; set; }
        public decimal CalculatedUnitPrice { get; set; }
        public decimal TotalCost { get; set; }

        public ProductPurchaseEntryEntity? ProductPurchaseEntry { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
