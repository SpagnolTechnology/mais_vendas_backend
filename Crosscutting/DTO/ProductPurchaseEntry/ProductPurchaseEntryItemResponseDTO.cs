using Crosscutting.DTO.Base;

namespace Crosscutting.DTO.ProductPurchaseEntry
{
    public class ProductPurchaseEntryItemResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal MarkupPercent { get; set; }
        public decimal CalculatedUnitPrice { get; set; }
        public decimal TotalCost { get; set; }
    }
}
