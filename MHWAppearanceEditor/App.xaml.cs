using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MHWAppearanceEditor.Services;
using MHWAppearanceEditor.ValueConverters;
using MHWAppearanceEditor.ViewModels;
using MHWAppearanceEditor.ViewModels.Tabs;
using MHWAppearanceEditor.Views;
using MHWAppearanceEditor.Views.Tabs;
using ReactiveUI;
using Serilog;
using Splat;
using System.Reflection;

namespace MHWAppearanceEditor
{
    public class App : Application, IEnableLogger
    {
        private static readonly Serilog.ILogger CtxLog = Log.ForContext<App>();

        public override void Initialize()
        {
            InitializeLogging();

            Locator.CurrentMutable.RegisterConstant(new ColorValueConverter(), typeof(IBindingTypeConverter));
            Locator.CurrentMutable.RegisterConstant(new SteamWebApiService(SuperSecret.STEAM_WEB_API_KEY), typeof(SteamWebApiService));

            Locator.CurrentMutable.Register(() => new StartScreenView(), typeof(IViewFor<StartScreenViewModel>));
            Locator.CurrentMutable.Register(() => new SaveDataView(), typeof(IViewFor<SaveDataViewModel>));
            Locator.CurrentMutable.Register(() => new ExceptionView(), typeof(IViewFor<ExceptionViewModel>));

            // Tabs
            Locator.CurrentMutable.Register(() => new SaveDataInfoView(), typeof(IViewFor<SaveDataInfoViewModel>));
            Locator.CurrentMutable.Register(() => new SaveSlotView(), typeof(IViewFor<SaveSlotViewModel>));

            CtxLog.Information("MHWAppearanceEditor v" + Assembly.GetExecutingAssembly().GetName().Version);
            CtxLog.Information("Cirilla.Core v" + Assembly.GetAssembly(typeof(Cirilla.Core.Models.GMD)).GetName().Version);

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };

            base.OnFrameworkInitializationCompleted();
        }

        private void InitializeLogging()
        {
            // Cirilla and MHWAppearanceEditor logging
            var logSink = new LogSink();
            var logger = new LoggerConfiguration()
                .WriteTo.Sink(logSink)
                .CreateLogger();
            Log.Logger = logger;
            Locator.CurrentMutable.RegisterConstant(logSink, typeof(LogSink)); // Could easily make an interface for this, if ever needed
        }
    }
}
