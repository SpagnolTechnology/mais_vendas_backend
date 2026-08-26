namespace Crosscutting.DTO.StockAdjustment
{
    public class UpdateStockAdjustmentItemRequestDTO
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
