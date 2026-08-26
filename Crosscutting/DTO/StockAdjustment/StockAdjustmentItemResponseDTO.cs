using Crosscutting.DTO.Base;

namespace Crosscutting.DTO.StockAdjustment
{
    public class StockAdjustmentItemResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
