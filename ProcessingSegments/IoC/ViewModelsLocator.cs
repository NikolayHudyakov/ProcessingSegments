using Microsoft.Extensions.DependencyInjection;
using ProcessingSegments.ViewModels;

namespace ProcessingSegments.IoC
{
    internal class ViewModelsLocator
    {
        public static MainWindowViewModel MainWindowViewModel => App.Host.Services.GetRequiredService<MainWindowViewModel>();
    }
}
