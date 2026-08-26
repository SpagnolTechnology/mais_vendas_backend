using Domain.Base;

namespace Domain.Entity
{
    public class TestEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}
