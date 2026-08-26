using Domain.Base;

namespace Domain.Entity
{
    public class ProductStockEntity : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? MinimumQuantity { get; set; }

        public ProductEntity? Product { get; set; }
    }
}
