using AppService.AutoMapping.Base;
using Crosscutting.DTO.Discount;
using Domain.Entity;

namespace AppService.AutoMapping.Discount
{
    public class DiscountProfile : BaseProfile
    {
        public DiscountProfile()
        {
            CreateMap<DiscountEntity, DiscountResponseDTO>();
        }
    }
}
