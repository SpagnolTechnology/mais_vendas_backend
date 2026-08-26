using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Repository.Services
{
    public class UnitOfMeasureRepository : GenericRepository<UnitOfMeasureEntity, DatabaseContext>, IUnitOfMeasureRepository
    {
        public UnitOfMeasureRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
