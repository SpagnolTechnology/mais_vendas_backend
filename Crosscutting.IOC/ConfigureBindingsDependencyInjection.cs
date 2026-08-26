using Crosscutting.IOC.Database;
using Crosscutting.IOC.Mapper;
using Crosscutting.IOC.Repository;
using Crosscutting.IOC.Services;
using Crosscutting.IOC.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IOC
{
    public static class ConfigureBindingsDependencyInjection
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database

            ConfigureBindingsDatabase.RegisterBindings(services, configuration);

            #endregion

            #region Validation

            ConfigureBindingsValidation.RegisterBindings(services);

            #endregion

            #region Repository

            ConfigureBindingsRepository.RegisterBindings(services);

            #endregion

            #region Service

            ConfigureBindingsService.RegisterBindings(services, configuration);

            #endregion

            #region Mapper

            ConfigureBindingsMapper.RegisterBindings(services);

            #endregion
        }
    }
}
