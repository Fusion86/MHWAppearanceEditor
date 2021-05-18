using Avalonia;
using Avalonia.ReactiveUI;
using Serilog;
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            // The real main needs to be in a different method, see https://stackoverflow.com/a/25990979/2125072
            return RealMain(args);
        }

        private static int RealMain(string[] args)
        {
            return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            var config = AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

            // Use Direct2D where possible, because it supports japanese, chinese, korean, etc.. characters
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                config.UseDirect2D1();

            return config;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            var probingPath = Path.GetFullPath("lib");
            var assyName = new AssemblyName(args.Name!);

            var dllPath = Path.Combine(probingPath, assyName.Name!);
            if (!dllPath.EndsWith(".dll"))
                dllPath += ".dll";

            if (File.Exists(dllPath))
                return Assembly.LoadFile(dllPath);

            return null!;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            Log.Error(ex, $"Unhandled exception: {ex?.Message}");
        }
    }
}
