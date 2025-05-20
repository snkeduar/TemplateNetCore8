using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace CustomLoggin
{
    public static class ConfigurarTelemetry
    {
        public static void ConfigurarServiciosTelemetry(this IServiceCollection services)
        {
            var aiOptions = new ApplicationInsightsServiceOptions
            {
                EnablePerformanceCounterCollectionModule = false
            };

            services.AddSingleton<ITelemetryInitializer, CustomInitializer>();
            services.AddApplicationInsightsTelemetry(aiOptions);
        }
    }
}
