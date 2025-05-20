using Microsoft.Extensions.DependencyInjection;

namespace CommonServices
{
    public static class InitCommonServices
    {
        public static void ConfigurarCommonServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUtilsService, UtilsService>();
        }
    }
}
