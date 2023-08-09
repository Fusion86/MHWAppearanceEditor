using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MHWAppearanceEditor.Services;
using MHWAppearanceEditor.ValueConverters;
using MHWAppearanceEditor.ViewModels;
using MHWAppearanceEditor.ViewModels.Tabs;
using MHWAppearanceEditor.Views;
using MHWAppearanceEditor.Views.Components;
using MHWAppearanceEditor.Views.Tabs;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MHWAppearanceEditor
{
    public class App : Application, IEnableLogger
    {
        public override void Initialize()
        {
            string writableAppDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHWAppearanceEditor");
            Directory.CreateDirectory(writableAppDir);
            InitializeLogging(writableAppDir);

            RxApp.DefaultExceptionHandler = new RxExceptionHandler();

            // Need to call ForContext *afer* calling InitializeLogging()
            var log = Log.ForContext<App>();
            log.Information("AppData directory: {Dir}", writableAppDir);
            log.Information("MHWAppearanceEditor v" + Assembly.GetExecutingAssembly()!.GetName().Version);
            log.Information("Cirilla.Core v" + Assembly.GetAssembly(typeof(Cirilla.Core.Models.SaveData))!.GetName().Version);

            // Settings
            var settingsService = new SettingsService(Path.Combine(writableAppDir, "config.json"));
            Locator.CurrentMutable.RegisterConstant(settingsService);

            // Backup service
            Locator.CurrentMutable.RegisterConstant(new BackupService(Path.Combine(writableAppDir, "backup")));

            // Start assets loading
            string appdir = Environment.GetEnvironmentVariable("APPDIR") ?? ".";
            AssetsService assetsService = new AssetsService(Path.Combine(appdir, "assets"));
            Task.Run(() => assetsService.Initialize());
            Locator.CurrentMutable.RegisterConstant(assetsService);

            Locator.CurrentMutable.RegisterConstant(new ColorValueConverter(), typeof(IBindingTypeConverter));
            Locator.CurrentMutable.RegisterConstant(new SteamWebApiService(SuperSecret.STEAM_WEB_API_KEY));

            Locator.CurrentMutable.Register(() => new StartScreenView(), typeof(IViewFor<StartScreenViewModel>));
            Locator.CurrentMutable.Register(() => new SaveDataView(), typeof(IViewFor<SaveDataViewModel>));
            Locator.CurrentMutable.Register(() => new ExceptionView(), typeof(IViewFor<ExceptionViewModel>));
            Locator.CurrentMutable.Register(() => new CharacterAssetView(), typeof(IViewFor<CharacterAssetViewModel>));

            // Tabs
            Locator.CurrentMutable.Register(() => new SaveDataInfoView(), typeof(IViewFor<SaveDataInfoViewModel>));
            Locator.CurrentMutable.Register(() => new SaveSlotView(), typeof(IViewFor<SaveSlotViewModel>));

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };

            base.OnFrameworkInitializationCompleted();
        }

        private static void InitializeLogging(string logDir)
        {
            // Cirilla and MHWAppearanceEditor logging
            var logSink = new LogSink();

            if (!Design.IsDesignMode)
            {
                var logger = new LoggerConfiguration()
                .WriteTo.Sink(logSink)
                .WriteTo.File(Path.Combine(logDir, "MHWAppearanceEditor-.log"), rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] <{SourceContext}> {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
                Log.Logger = logger;
            }

            Locator.CurrentMutable.RegisterConstant(logSink, typeof(LogSink)); // Could easily make an interface for this, if ever needed
        }
    }
}
