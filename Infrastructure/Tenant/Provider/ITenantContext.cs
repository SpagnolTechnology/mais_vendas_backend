namespace Infrastructure.Tenant.Provider
{
    public interface ITenantContext
    {
        string? CurrentCnpj { get; }

        void SetCurrentCnpj(string cnpj);

        void Clear();
    }
}
