using Microsoft.Extensions.DependencyInjection;
using ProcessingSegments.Models.Interfaces;
using ProcessingSegments.Services;
using ProcessingSegments.Services.Interfaces;

namespace ProcessingSegments.IoC
{
    internal static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddSingleton<IObjectProviderService<List<Point>>, FileDialogDeserializer<List<Point>>>();
    }
}
