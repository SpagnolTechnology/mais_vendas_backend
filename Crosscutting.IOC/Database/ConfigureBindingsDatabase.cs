using Infrastructure.Context;
using Infrastructure.Tenant.Factory;
using Infrastructure.Tenant.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IOC.Database
{
    public static class ConfigureBindingsDatabase
    {
        public static void RegisterBindings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITenantConnectionStringProvider, TenantConnectionStringProvider>();
            services.AddScoped<ITenantContext, TenantContext>();
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddSingleton<ITenantDbContextFactory, TenantDbContextFactory>();

            services.AddDbContextPool<DatabaseContextAdmin>((serviceProvider, options) =>
            {
                ITenantConnectionStringProvider connectionStringProvider = serviceProvider.GetRequiredService<ITenantConnectionStringProvider>();
                string connectionString = connectionStringProvider.GetAdminConnectionString();

                options.UseNpgsql(connectionString);
            });

            services.AddScoped(provider =>
            {
                ITenantDbContextFactory contextFactory = provider.GetRequiredService<ITenantDbContextFactory>();
                return contextFactory.CreateContext();
            });
        }
    }
}
