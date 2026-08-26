namespace Infrastructure.Repository.Interfaces
{
    public interface ISalesOperationsRepository
    {
        Task ConfirmPurchaseEntryAsync(int entryId, string userEmail, DateTime now, CancellationToken ct = default);
        Task ConfirmStockAdjustmentAsync(int adjustmentId, string userEmail, DateTime now, CancellationToken ct = default);
        Task ConfirmSaleAsync(int saleId, string userEmail, DateTime now, CancellationToken ct = default);
    }
}
