using Crosscutting.Enum;
using Domain.Base;

namespace Domain.Entity
{
    public class ProductPurchaseEntryEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public int SupplierId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceSeries { get; set; } = string.Empty;
        public string? InvoiceKey { get; set; }
        public PurchaseEntryStatusEnum Status { get; set; } = PurchaseEntryStatusEnum.Draft;
        public DateTime EntryDate { get; set; }
        public string? Notes { get; set; }

        public SupplierEntity? Supplier { get; set; }
        public ICollection<ProductPurchaseEntryItemEntity> Items { get; set; } = new List<ProductPurchaseEntryItemEntity>();
    }
}
