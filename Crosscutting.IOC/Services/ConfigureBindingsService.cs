using AppService.AppService.External;
using AppService.AppService.Interfaces;
using AppService.AppService.Services;
using Crosscutting.Configuration;
using Crosscutting.External;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IOC.Services
{
    public static class ConfigureBindingsService
    {
        public static void RegisterBindings(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DomainsOptions>(configuration.GetSection(DomainsOptions.SectionName));

            services.AddHttpClient<IExternalClientService, ExternalClientService>();
            services.AddHttpClient<IExternalBillingService, ExternalBillingService>();

            services.AddScoped<ITestAppService, TestAppService>();
            services.AddScoped<ISupplierAppService, SupplierAppService>();
            services.AddScoped<IUnitOfMeasureAppService, UnitOfMeasureAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IPaymentConditionAppService, PaymentConditionAppService>();
            services.AddScoped<IDiscountRuleAppService, DiscountRuleAppService>();
            services.AddScoped<ICommissionRuleAppService, CommissionRuleAppService>();
            services.AddScoped<IProductPurchaseEntryAppService, ProductPurchaseEntryAppService>();
            services.AddScoped<IStockAdjustmentAppService, StockAdjustmentAppService>();
            services.AddScoped<IProposalAppService, ProposalAppService>();
            services.AddScoped<IProposalAuthenticationAppService, ProposalAuthenticationAppService>();
            services.AddScoped<ISaleAppService, SaleAppService>();
            services.AddScoped<IDiscountAppService, DiscountAppService>();
            services.AddScoped<ICommissionAppService, CommissionAppService>();
        }
    }
}
