using Infrastructure.Context;

namespace Infrastructure.Tenant.Factory
{
    public interface ITenantDbContextFactory
    {
        DatabaseContext CreateContext();
        DatabaseContext CreateContext(string cnpj);
    }
}
