using AppService.AutoMapping.Base;
using Crosscutting.DTO.Product;
using Domain.Entity;

namespace AppService.AutoMapping.Product
{
    public class ProductProfile : BaseProfile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequestDTO, ProductEntity>();
            CreateMap<ProductEntity, ProductResponseDTO>();
        }
    }
}
