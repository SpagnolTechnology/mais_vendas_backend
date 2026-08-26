using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Repository.Services
{
    public class CommissionRepository : GenericRepository<CommissionEntity, DatabaseContext>, ICommissionRepository
    {
        public CommissionRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
