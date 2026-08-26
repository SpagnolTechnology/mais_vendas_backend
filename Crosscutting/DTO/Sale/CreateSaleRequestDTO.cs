using Crosscutting.Enum;

namespace Crosscutting.DTO.Sale
{
    public class CreateSaleRequestDTO
    {
        public int? ProposalId { get; set; }
        public string ExternalClientId { get; set; } = string.Empty;
        public int PaymentConditionId { get; set; }
        public SaleTypeEnum SaleType { get; set; }
        public List<CreateSaleItemRequestDTO> Items { get; set; } = new();
    }
}
