using Crosscutting.DTO.Base;

namespace Crosscutting.DTO.StockAdjustment
{
    public class StockAdjustmentResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime AdjustmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public bool IsConfirmed { get; set; }
        public List<StockAdjustmentItemResponseDTO> Items { get; set; } = new();
    }
}
