using AppService.AutoMapping.Base;
using Crosscutting.DTO.Sale;
using Domain.Entity;

namespace AppService.AutoMapping.Sale
{
    public class SaleProfile : BaseProfile
    {
        public SaleProfile()
        {
            CreateMap<SaleEntity, SaleResponseDTO>();
            CreateMap<SaleItemEntity, SaleItemResponseDTO>();
        }
    }
}
