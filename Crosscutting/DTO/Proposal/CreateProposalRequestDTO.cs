using Crosscutting.Enum;

namespace Crosscutting.DTO.Proposal
{
    public class CreateProposalRequestDTO
    {
        public string ExternalClientId { get; set; } = string.Empty;
        public int PaymentConditionId { get; set; }
        public SaleTypeEnum SaleType { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string? Notes { get; set; }
        public List<CreateProposalItemRequestDTO> Items { get; set; } = new();
    }
}
