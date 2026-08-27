using AppService.AutoMapping.Base;
using Crosscutting.DTO.Product;
using Domain.Entity;

namespace AppService.AutoMapping.Product
{
    public class ProductPhotoProfile : BaseProfile
    {
        public ProductPhotoProfile()
        {
            CreateMap<CreateProductPhotoRequestDTO, ProductPhotoEntity>();
            CreateMap<ProductPhotoEntity, ProductPhotoResponseDTO>();
        }
    }
}
