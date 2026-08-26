using Crosscutting.DTO.Base;
using Crosscutting.Enum;

namespace Crosscutting.DTO.StockMovement
{
    public class StockMovementResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public StockMovementTypeEnum MovementType { get; set; }
        public decimal Quantity { get; set; }
        public decimal BalanceAfter { get; set; }
        public int? ProductPurchaseEntryItemId { get; set; }
        public int? StockAdjustmentItemId { get; set; }
        public int? SaleId { get; set; }
        public string? OriginDocumentNumber { get; set; }
        public string? Notes { get; set; }
    }
}
