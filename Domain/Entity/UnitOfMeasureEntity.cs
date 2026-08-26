using Domain.Base;

namespace Domain.Entity
{
    public class UnitOfMeasureEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
