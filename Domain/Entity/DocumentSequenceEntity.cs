using Crosscutting.Enum;
using Domain.Base;

namespace Domain.Entity
{
    public class DocumentSequenceEntity : BaseEntity
    {
        public int Id { get; set; }
        public DocumentTypeEnum DocumentType { get; set; }
        public int Year { get; set; }
        public int LastNumber { get; set; }
    }
}
