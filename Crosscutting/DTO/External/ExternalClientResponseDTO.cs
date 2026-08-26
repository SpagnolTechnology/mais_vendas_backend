namespace Crosscutting.DTO.External
{
    public class ExternalClientResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
