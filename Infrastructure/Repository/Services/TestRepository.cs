using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Repository.Services
{
    public class TestRepository : GenericRepository<TestEntity, DatabaseContext>, ITestRepository
    {
        public TestRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
