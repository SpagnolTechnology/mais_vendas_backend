using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Repository.Services
{
    public class SupplierRepository : GenericRepository<SupplierEntity, DatabaseContext>, ISupplierRepository
    {
        public SupplierRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
