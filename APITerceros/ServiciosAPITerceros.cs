using APITerceros.SeguridadApi;
using Microsoft.Extensions.DependencyInjection;

namespace APITerceros
{
    public static class ServiciosAPITerceros
    {
        public static void ConfigurarServiciosAPITerceros(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRestClientService, RestClientService>();
            serviceCollection.AddTransient<ISeguridadService, SeguridadService>();
        }
    }
}