using Crosscutting.Enum;

namespace Crosscutting.DTO.Proposal
{
    public class UpdateProposalRequestDTO
    {
        public string ExternalClientId { get; set; } = string.Empty;
        public int PaymentConditionId { get; set; }
        public SaleTypeEnum SaleType { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string? Notes { get; set; }
        public List<UpdateProposalItemRequestDTO> Items { get; set; } = new();
    }
}
