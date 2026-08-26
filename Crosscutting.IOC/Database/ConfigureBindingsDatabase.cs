using Infrastructure.Context;
using Infrastructure.Tenant.Factory;
using Infrastructure.Tenant.Provider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IOC.Database
{
    public static class ConfigureBindingsDatabase
    {
        public static void RegisterBindings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITenantConnectionStringProvider, TenantConnectionStringProvider>();
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddSingleton<ITenantDbContextFactory, TenantDbContextFactory>();

            services.AddScoped(provider =>
            {
                ITenantDbContextFactory contextFactory = provider.GetRequiredService<ITenantDbContextFactory>();
                return contextFactory.CreateContext();
            });
        }
    }
}
