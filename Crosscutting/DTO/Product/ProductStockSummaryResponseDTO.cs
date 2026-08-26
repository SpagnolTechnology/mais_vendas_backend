namespace Crosscutting.DTO.Product
{
    public class ProductStockSummaryResponseDTO
    {
        public int ProductId { get; set; }
        public decimal Physical { get; set; }
        public decimal Reserved { get; set; }
        public decimal Available { get; set; }
    }
}
