namespace Crosscutting.DTO.StockAdjustment
{
    public class CreateStockAdjustmentRequestDTO
    {
        public DateTime AdjustmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public List<CreateStockAdjustmentItemRequestDTO> Items { get; set; } = new();
    }
}
