namespace Crosscutting.DTO.NfeImport
{
    public class ParsedInvoiceDTO
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceSeries { get; set; } = string.Empty;
        public string? InvoiceKey { get; set; }
        public DateTime EntryDate { get; set; }
        public ParsedInvoiceSupplierDTO Supplier { get; set; } = new();
        public List<ParsedInvoiceItemDTO> Items { get; set; } = new();
    }
}
