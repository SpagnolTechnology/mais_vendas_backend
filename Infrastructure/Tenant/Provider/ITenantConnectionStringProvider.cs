namespace Infrastructure.Tenant.Provider
{
    public interface ITenantConnectionStringProvider
    {
        string GetConnectionString(string cnpj);

        string GetAdminConnectionString();
    }
}
