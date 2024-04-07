using Microsoft.Extensions.DependencyInjection;
using ProcessingSegments.ViewModels;

namespace ProcessingSegments.IoC
{
    internal static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModel(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>();
    }
}
