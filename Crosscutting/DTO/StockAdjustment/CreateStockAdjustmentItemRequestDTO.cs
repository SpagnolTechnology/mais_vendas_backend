namespace Crosscutting.DTO.StockAdjustment
{
    public class CreateStockAdjustmentItemRequestDTO
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
