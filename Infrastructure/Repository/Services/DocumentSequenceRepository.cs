using Crosscutting.Enum;
using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class DocumentSequenceRepository : GenericRepository<DocumentSequenceEntity, DatabaseContext>, IDocumentSequenceRepository
    {
        public DocumentSequenceRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<string> GetNextNumberAsync(DocumentTypeEnum type, int year, CancellationToken ct = default)
        {
            var set = _context.Set<DocumentSequenceEntity>();
            var sequence = await set.FirstOrDefaultAsync(x => x.DocumentType == type && x.Year == year, ct);

            if (sequence == null)
            {
                sequence = new DocumentSequenceEntity
                {
                    DocumentType = type,
                    Year = year,
                    LastNumber = 0,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                };

                await set.AddAsync(sequence, ct);
            }

            sequence.LastNumber++;

            return $"{year}/{sequence.LastNumber:D6}";
        }
    }
}
