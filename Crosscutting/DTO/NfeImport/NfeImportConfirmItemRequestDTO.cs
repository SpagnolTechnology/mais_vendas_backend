namespace Crosscutting.DTO.NfeImport
{
    public class NfeImportConfirmItemRequestDTO
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal MarkupPercent { get; set; }
    }
}
