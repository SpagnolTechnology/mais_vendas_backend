using Crosscutting.DTO.Base;
using Crosscutting.Enum;

namespace Crosscutting.DTO.ProductPurchaseEntry
{
    public class ProductPurchaseEntryResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public int SupplierId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceSeries { get; set; } = string.Empty;
        public string? InvoiceKey { get; set; }
        public PurchaseEntryStatusEnum Status { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Notes { get; set; }
        public List<ProductPurchaseEntryItemResponseDTO> Items { get; set; } = new();
    }
}
