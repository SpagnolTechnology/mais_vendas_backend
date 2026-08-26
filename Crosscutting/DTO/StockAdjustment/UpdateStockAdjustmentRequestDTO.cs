namespace Crosscutting.DTO.StockAdjustment
{
    public class UpdateStockAdjustmentRequestDTO
    {
        public DateTime AdjustmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public List<UpdateStockAdjustmentItemRequestDTO> Items { get; set; } = new();
    }
}
