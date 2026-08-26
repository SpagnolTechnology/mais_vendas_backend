using Domain.Entity;
using Infrastructure.Base;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Repository.Services
{
    public class PaymentConditionRepository : GenericRepository<PaymentConditionEntity, DatabaseContext>, IPaymentConditionRepository
    {
        public PaymentConditionRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
