using AppService.AutoMapping.Base;
using Crosscutting.DTO.StockMovement;
using Domain.Entity;

namespace AppService.AutoMapping.StockMovement
{
    public class StockMovementProfile : BaseProfile
    {
        public StockMovementProfile()
        {
            CreateMap<StockMovementEntity, StockMovementResponseDTO>();
        }
    }
}
