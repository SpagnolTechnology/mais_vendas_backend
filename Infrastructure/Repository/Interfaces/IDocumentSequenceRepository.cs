using Crosscutting.Enum;
using Domain.Entity;
using Infrastructure.Base;

namespace Infrastructure.Repository.Interfaces
{
    public interface IDocumentSequenceRepository : IGenericRepository<DocumentSequenceEntity>
    {
        Task<string> GetNextNumberAsync(DocumentTypeEnum type, int year, CancellationToken ct = default);
    }
}
