namespace Crosscutting.DTO.NfeImport
{
    public class NfeImportConfirmRequestDTO
    {
        public int SupplierId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceSeries { get; set; } = string.Empty;
        public string? InvoiceKey { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Notes { get; set; }
        public List<NfeImportConfirmItemRequestDTO> Items { get; set; } = new();
    }
}
