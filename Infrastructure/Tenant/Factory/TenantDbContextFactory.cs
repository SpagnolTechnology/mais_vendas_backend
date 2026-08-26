using Infrastructure.Context;
using Infrastructure.Tenant.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infrastructure.Tenant.Factory
{
    public class TenantDbContextFactory : ITenantDbContextFactory, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITenantConnectionStringProvider _connectionStringProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ConcurrentDictionary<string, PooledDbContextFactory> _factoryCache;

        public TenantDbContextFactory(
            IServiceProvider serviceProvider,
            ITenantConnectionStringProvider connectionStringProvider,
            ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _connectionStringProvider = connectionStringProvider;
            _loggerFactory = loggerFactory;
            _factoryCache = new ConcurrentDictionary<string, PooledDbContextFactory>();
        }

        public DatabaseContext CreateContext()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            ITenantProvider tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
            string cnpj = tenantProvider.GetCurrentCnpj();

            return CreateContext(cnpj);
        }

        public DatabaseContext CreateContext(string cnpj)
        {
            PooledDbContextFactory pooledFactory = _factoryCache.GetOrAdd(cnpj, key =>
            {
                string connectionString = _connectionStringProvider.GetConnectionString(key);
                return new PooledDbContextFactory(connectionString, _loggerFactory);
            });

            return pooledFactory.CreateDbContext();
        }

        public void Dispose()
        {
            foreach (PooledDbContextFactory factory in _factoryCache.Values)
            {
                factory.Dispose();
            }

            _factoryCache.Clear();
        }

        private class PooledDbContextFactory : IDisposable
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly IDbContextFactory<DatabaseContext> _factory;

            public PooledDbContextFactory(string connectionString, ILoggerFactory loggerFactory)
            {
                ServiceCollection services = new();

                services.AddSingleton(loggerFactory);
                services.AddDbContextFactory<DatabaseContext>(
                    options => options.UseNpgsql(connectionString),
                    lifetime: ServiceLifetime.Singleton);

                _serviceProvider = services.BuildServiceProvider();
                _factory = _serviceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
            }

            public DatabaseContext CreateDbContext()
            {
                return _factory.CreateDbContext();
            }

            public void Dispose()
            {
                if (_serviceProvider is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}
