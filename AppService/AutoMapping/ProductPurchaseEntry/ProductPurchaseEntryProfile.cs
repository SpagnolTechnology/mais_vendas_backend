using AppService.AutoMapping.Base;
using Crosscutting.DTO.ProductPurchaseEntry;
using Domain.Entity;

namespace AppService.AutoMapping.ProductPurchaseEntry
{
    public class ProductPurchaseEntryProfile : BaseProfile
    {
        public ProductPurchaseEntryProfile()
        {
            CreateMap<CreateProductPurchaseEntryRequestDTO, ProductPurchaseEntryEntity>()
                .ForMember(dest => dest.Items, opt => opt.Ignore());
            CreateMap<CreateProductPurchaseEntryItemRequestDTO, ProductPurchaseEntryItemEntity>();
            CreateMap<ProductPurchaseEntryEntity, ProductPurchaseEntryResponseDTO>();
            CreateMap<ProductPurchaseEntryItemEntity, ProductPurchaseEntryItemResponseDTO>();
        }
    }
}
