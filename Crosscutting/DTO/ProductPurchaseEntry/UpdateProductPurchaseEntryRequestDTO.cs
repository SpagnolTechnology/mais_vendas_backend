namespace Crosscutting.DTO.ProductPurchaseEntry
{
    public class UpdateProductPurchaseEntryRequestDTO
    {
        public int SupplierId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceSeries { get; set; } = string.Empty;
        public string? InvoiceKey { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Notes { get; set; }
        public List<UpdateProductPurchaseEntryItemRequestDTO> Items { get; set; } = new();
    }
}
