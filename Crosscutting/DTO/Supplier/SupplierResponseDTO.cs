using Crosscutting.DTO.Base;

namespace Crosscutting.DTO.Supplier
{
    public class SupplierResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public bool IsActive { get; set; }
    }
}
