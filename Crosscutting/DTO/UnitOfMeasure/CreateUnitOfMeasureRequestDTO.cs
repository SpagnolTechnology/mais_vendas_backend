namespace Crosscutting.DTO.UnitOfMeasure
{
    public class CreateUnitOfMeasureRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
