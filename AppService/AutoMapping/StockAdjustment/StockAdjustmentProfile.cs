using AppService.AutoMapping.Base;
using Crosscutting.DTO.StockAdjustment;
using Domain.Entity;

namespace AppService.AutoMapping.StockAdjustment
{
    public class StockAdjustmentProfile : BaseProfile
    {
        public StockAdjustmentProfile()
        {
            CreateMap<CreateStockAdjustmentRequestDTO, StockAdjustmentEntity>();
            CreateMap<CreateStockAdjustmentItemRequestDTO, StockAdjustmentItemEntity>();
            CreateMap<StockAdjustmentEntity, StockAdjustmentResponseDTO>();
            CreateMap<StockAdjustmentItemEntity, StockAdjustmentItemResponseDTO>();
        }
    }
}
