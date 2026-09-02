namespace Crosscutting.DTO.NfeImport
{
    public class NfeImportPreviewResponseDTO
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceSeries { get; set; } = string.Empty;
        public string? InvoiceKey { get; set; }
        public DateTime EntryDate { get; set; }
        public NfeImportSupplierMatchDTO SupplierMatch { get; set; } = new();
        public List<NfeImportItemPreviewDTO> Items { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public bool IsDuplicateInvoiceKey { get; set; }
    }
}
