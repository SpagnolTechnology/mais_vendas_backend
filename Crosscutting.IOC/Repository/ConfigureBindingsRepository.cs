using Infrastructure.Repository.Interfaces;
using Infrastructure.Repository.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IOC.Repository
{
    public static class ConfigureBindingsRepository
    {
        public static void RegisterBindings(IServiceCollection services)
        {
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IUnitOfMeasureRepository, UnitOfMeasureRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductStockRepository, ProductStockRepository>();
            services.AddScoped<IPaymentConditionRepository, PaymentConditionRepository>();
            services.AddScoped<IDiscountRuleRepository, DiscountRuleRepository>();
            services.AddScoped<ICommissionRuleRepository, CommissionRuleRepository>();
            services.AddScoped<IProductPurchaseEntryRepository, ProductPurchaseEntryRepository>();
            services.AddScoped<IProductPurchaseEntryItemRepository, ProductPurchaseEntryItemRepository>();
            services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();
            services.AddScoped<IStockMovementRepository, StockMovementRepository>();
            services.AddScoped<IProposalRepository, ProposalRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<ICommissionRepository, CommissionRepository>();
            services.AddScoped<IDocumentSequenceRepository, DocumentSequenceRepository>();
            services.AddScoped<ISalesOperationsRepository, SalesOperationsRepository>();
        }
    }
}
