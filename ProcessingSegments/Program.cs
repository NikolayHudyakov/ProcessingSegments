using System.Diagnostics;
using System.Reflection;

namespace ProcessingSegments
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var processName = Assembly.GetExecutingAssembly().GetName().Name;

            if (Process.GetCurrentProcess().ProcessName != processName || 
                Process.GetProcessesByName(processName).Length != 1)
                return;

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
