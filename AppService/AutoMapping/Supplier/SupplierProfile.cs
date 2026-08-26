using AppService.AutoMapping.Base;
using Crosscutting.DTO.Supplier;
using Domain.Entity;

namespace AppService.AutoMapping.Supplier
{
    public class SupplierProfile : BaseProfile
    {
        public SupplierProfile()
        {
            CreateMap<CreateSupplierRequestDTO, SupplierEntity>();
            CreateMap<SupplierEntity, SupplierResponseDTO>();
        }
    }
}
