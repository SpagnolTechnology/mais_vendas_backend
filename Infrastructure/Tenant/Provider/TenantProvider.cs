using Crosscutting.Helpers;
using Infrastructure.Tenant.Provider;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Tenant.Provider
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITenantContext _tenantContext;

        public TenantProvider(IHttpContextAccessor httpContextAccessor, ITenantContext tenantContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenantContext = tenantContext;
        }

        public string GetCurrentCnpj()
        {
            if (!string.IsNullOrWhiteSpace(_tenantContext.CurrentCnpj))
                return _tenantContext.CurrentCnpj;

            return JwtHelper.GetCnpjByToken(_httpContextAccessor);
        }
    }
}
