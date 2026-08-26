using Crosscutting.Helpers;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Tenant.Provider
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentCnpj()
        {
            return JwtHelper.GetCnpjByToken(_httpContextAccessor);
        }
    }
}
