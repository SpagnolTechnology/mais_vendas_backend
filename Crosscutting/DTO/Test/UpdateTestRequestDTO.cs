namespace Crosscutting.DTO.Test
{
    public class UpdateTestRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}
