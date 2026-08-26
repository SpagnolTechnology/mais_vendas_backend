using Crosscutting.DTO.Base;

namespace Crosscutting.DTO.UnitOfMeasure
{
    public class UnitOfMeasureResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
