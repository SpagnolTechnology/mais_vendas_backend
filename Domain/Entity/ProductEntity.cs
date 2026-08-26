using Domain.Base;

namespace Domain.Entity
{
    public class ProductEntity : BaseEntity
    {
        public int Id { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal MarkupPercent { get; set; }
        public decimal IcmsPercent { get; set; }
        public decimal IssPercent { get; set; }
        public decimal PisPercent { get; set; }
        public decimal CofinsPercent { get; set; }
        public bool IsActive { get; set; } = true;

        public UnitOfMeasureEntity? UnitOfMeasure { get; set; }
        public ProductStockEntity? ProductStock { get; set; }
    }
}
