using Microsoft.AspNetCore.Http;

namespace Infrastructure.Tenant.Provider
{
    public class TenantContext : ITenantContext
    {
        private const string CurrentCnpjKey = "CurrentTenantCnpj";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _currentCnpj;

        public TenantContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? CurrentCnpj
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.Items.TryGetValue(CurrentCnpjKey, out object? value) == true)
                    return value?.ToString();

                return _currentCnpj;
            }
        }

        public void SetCurrentCnpj(string cnpj)
        {
            _currentCnpj = cnpj;

            if (_httpContextAccessor.HttpContext is not null)
                _httpContextAccessor.HttpContext.Items[CurrentCnpjKey] = cnpj;
        }

        public void Clear()
        {
            _currentCnpj = null;
            _httpContextAccessor.HttpContext?.Items.Remove(CurrentCnpjKey);
        }
    }
}
