using Microsoft.Extensions.DependencyInjection;
using ProcessingSegments.Models;
using ProcessingSegments.Models.Interfaces;

namespace ProcessingSegments.IoC
{
    internal static class ModelsRegistrator
    {
        public static IServiceCollection AddModels(this IServiceCollection services) => services
            .AddSingleton<IModel, Model>();
            
    }
}
