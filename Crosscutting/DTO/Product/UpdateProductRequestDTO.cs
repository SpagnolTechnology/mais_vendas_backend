namespace Crosscutting.DTO.Product
{
    public class UpdateProductRequestDTO
    {
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
        public bool IsActive { get; set; }
    }
}
