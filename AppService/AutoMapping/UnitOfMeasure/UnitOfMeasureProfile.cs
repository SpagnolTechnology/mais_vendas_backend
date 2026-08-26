using AppService.AutoMapping.Base;
using Crosscutting.DTO.UnitOfMeasure;
using Domain.Entity;

namespace AppService.AutoMapping.UnitOfMeasure
{
    public class UnitOfMeasureProfile : BaseProfile
    {
        public UnitOfMeasureProfile()
        {
            CreateMap<CreateUnitOfMeasureRequestDTO, UnitOfMeasureEntity>();
            CreateMap<UnitOfMeasureEntity, UnitOfMeasureResponseDTO>();
        }
    }
}
