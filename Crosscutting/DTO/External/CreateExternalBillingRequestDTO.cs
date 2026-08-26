namespace Crosscutting.DTO.External
{
    public class CreateExternalBillingRequestDTO
    {
        public int SaleId { get; set; }
        public string ExternalClientId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string SellerEmail { get; set; } = string.Empty;
        public string SaleNumber { get; set; } = string.Empty;
    }
}
