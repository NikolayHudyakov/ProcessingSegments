using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProcessingSegments.IoC;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProcessingSegments
{
    public partial class App : Application
    {
        private static IHost? _host;

        public static bool IsDesignMode { get; private set; } = true;
        public static IHost Host => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        public static string CurrentDirectory => IsDesignMode ? Path.GetDirectoryName(GetSourceCodePath()) ?? string.Empty : 
            AppContext.BaseDirectory;

        public static IHostBuilder CreateHostBuilder(string[] args) => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .UseContentRoot(CurrentDirectory)
            .ConfigureServices(ConfigureServices);

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddServices()
            .AddViewModel()
            .AddModels();

        protected override async void OnStartup(StartupEventArgs e)
        {
            IsDesignMode = false;
            base.OnStartup(e);
            await Host.RunAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            using (Host) await Host.StopAsync().ConfigureAwait(false);
        }

        private static string? GetSourceCodePath([CallerFilePath] string? path = null) => path;
    }
}
