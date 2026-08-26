namespace Crosscutting.DTO.ProductPurchaseEntry
{
    public class CreateProductPurchaseEntryItemRequestDTO
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal MarkupPercent { get; set; }
    }
}
