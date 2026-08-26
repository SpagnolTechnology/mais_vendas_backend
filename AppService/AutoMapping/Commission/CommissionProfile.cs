using AppService.AutoMapping.Base;
using Crosscutting.DTO.Commission;
using Domain.Entity;

namespace AppService.AutoMapping.Commission
{
    public class CommissionProfile : BaseProfile
    {
        public CommissionProfile()
        {
            CreateMap<CommissionEntity, CommissionResponseDTO>();
        }
    }
}
