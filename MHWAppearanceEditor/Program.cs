using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using System.Reflection;

namespace MHWAppearanceEditor
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static int Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            // The real main needs to be in a different method, see https://stackoverflow.com/a/25990979/2125072
            return RealMain(args);
        }

        private static int RealMain(string[] args)
        {
            return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var probingPath = Path.GetFullPath("lib");
            var assyName = new AssemblyName(args.Name);

            var dllPath = Path.Combine(probingPath, assyName.Name);
            if (!dllPath.EndsWith(".dll"))
                dllPath += ".dll";

            if (File.Exists(dllPath))
                return Assembly.LoadFile(dllPath);

            return null;
        }
    }
}
