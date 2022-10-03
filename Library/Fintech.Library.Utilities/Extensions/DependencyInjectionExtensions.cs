using Microsoft.Extensions.DependencyInjection;

namespace Fintech.Library.Utilities.Extensions;

public static class DependencyInjectionExtensions
{
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; set; }


        public static IServiceCollection Create(IServiceCollection services)
        {

            ServiceProvider = services.BuildServiceProvider();

            return services;
        }
    }
}
