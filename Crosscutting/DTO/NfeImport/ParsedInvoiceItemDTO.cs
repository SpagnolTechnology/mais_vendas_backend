namespace Crosscutting.DTO.NfeImport
{
    public class ParsedInvoiceItemDTO
    {
        public int LineNumber { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string? Ean { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
    }
}
