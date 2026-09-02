using Crosscutting.Helpers;
using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Services
{
    public class SupplierRepository : GenericRepository<SupplierEntity, DatabaseContext>, ISupplierRepository
    {
        public SupplierRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<SupplierEntity?> GetByDocumentAsync(string document, CancellationToken ct = default)
        {
            string normalizedDocument = DocumentHelper.NormalizeDigits(document);
            if (string.IsNullOrEmpty(normalizedDocument))
                return null;

            List<SupplierEntity> suppliers = await _context.Set<SupplierEntity>().ToListAsync(ct);
            return suppliers.FirstOrDefault(s => DocumentHelper.NormalizeDigits(s.Document) == normalizedDocument);
        }
    }
}
