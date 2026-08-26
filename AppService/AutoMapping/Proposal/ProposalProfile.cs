using AppService.AutoMapping.Base;
using Crosscutting.DTO.Proposal;
using Domain.Entity;

namespace AppService.AutoMapping.Proposal
{
    public class ProposalProfile : BaseProfile
    {
        public ProposalProfile()
        {
            CreateMap<ProposalEntity, ProposalResponseDTO>();
            CreateMap<ProposalItemEntity, ProposalItemResponseDTO>();
        }
    }
}
