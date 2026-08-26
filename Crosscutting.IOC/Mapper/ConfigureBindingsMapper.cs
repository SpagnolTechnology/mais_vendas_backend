using AppService.AutoMapping.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.IOC.Mapper
{
    public static class ConfigureBindingsMapper
    {
        public static void RegisterBindings(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(BaseProfile));
        }
    }
}
