using Domain.Base;

namespace Domain.Entity
{
    public class ProductPhotoEntity : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public ProductEntity? Product { get; set; }
    }
}
