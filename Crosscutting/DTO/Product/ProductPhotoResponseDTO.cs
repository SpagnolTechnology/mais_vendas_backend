using Crosscutting.DTO.Base;

namespace Crosscutting.DTO.Product
{
    public class ProductPhotoResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
