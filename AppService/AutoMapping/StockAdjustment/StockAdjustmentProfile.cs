using AppService.AutoMapping.Base;
using Crosscutting.DTO.StockAdjustment;
using Domain.Entity;

namespace AppService.AutoMapping.StockAdjustment
{
    public class StockAdjustmentProfile : BaseProfile
    {
        public StockAdjustmentProfile()
        {
            CreateMap<CreateStockAdjustmentRequestDTO, StockAdjustmentEntity>()
                .ForMember(dest => dest.Items, opt => opt.Ignore());
            CreateMap<CreateStockAdjustmentItemRequestDTO, StockAdjustmentItemEntity>();
            CreateMap<StockAdjustmentEntity, StockAdjustmentResponseDTO>();
            CreateMap<StockAdjustmentItemEntity, StockAdjustmentItemResponseDTO>();
        }
    }
}
