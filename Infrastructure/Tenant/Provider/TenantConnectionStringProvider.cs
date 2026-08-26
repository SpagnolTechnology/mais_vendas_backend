using Crosscutting.CustomException;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Net;

namespace Infrastructure.Tenant.Provider
{
    public class TenantConnectionStringProvider : ITenantConnectionStringProvider
    {
        private const string CompanyCnpjPlaceholder = "COMPANY_CNPJ";
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, string> _connectionStringCache;

        public TenantConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionStringCache = new ConcurrentDictionary<string, string>();
        }

        public string GetConnectionString(string cnpj)
        {
            string normalizedCnpj = NormalizeCnpj(cnpj);

            return _connectionStringCache.GetOrAdd(normalizedCnpj, key =>
            {
                string template = GetRequiredConnectionString("dbconnection");
                return template.Replace(CompanyCnpjPlaceholder, key);
            });
        }

        private static string NormalizeCnpj(string cnpj)
        {
            string normalizedCnpj = new(cnpj.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(normalizedCnpj))
                throw new CustomBusinessException("CNPJ do tenant não informado no token.", HttpStatusCode.Unauthorized);

            return normalizedCnpj;
        }

        private string GetRequiredConnectionString(string name)
        {
            string? value = _configuration.GetConnectionString(name);

            if (string.IsNullOrWhiteSpace(value))
                throw new CustomBusinessException($"Connection string '{name}' não encontrada.", HttpStatusCode.InternalServerError);

            return value;
        }
    }
}
