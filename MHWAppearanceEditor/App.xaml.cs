using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MHWAppearanceEditor.ViewModels;
using MHWAppearanceEditor.ViewModels.SaveSlotEditors;
using MHWAppearanceEditor.ViewModels.Tabs;
using MHWAppearanceEditor.Views;
using MHWAppearanceEditor.Views.SaveSlotEditors;
using MHWAppearanceEditor.Views.Tabs;
using ReactiveUI;
using Serilog;
using Splat;
using System.Reflection;

namespace MHWAppearanceEditor
{
    public class App : Application
    {
        public override void Initialize()
        {
            InitializeLogging();

            Locator.CurrentMutable.Register(() => new StartScreenView(), typeof(IViewFor<StartScreenViewModel>));
            Locator.CurrentMutable.Register(() => new SaveDataView(), typeof(IViewFor<SaveDataViewModel>));
            Locator.CurrentMutable.Register(() => new ExceptionView(), typeof(IViewFor<ExceptionViewModel>));

            // Tabs
            Locator.CurrentMutable.Register(() => new SaveDataInfoView(), typeof(IViewFor<SaveDataInfoViewModel>));
            Locator.CurrentMutable.Register(() => new SaveSlotView(), typeof(IViewFor<SaveSlotViewModel>));

            // SaveSlotEditors
            Locator.CurrentMutable.Register(() => new SaveSlotInfoView(), typeof(IViewFor<SaveSlotInfoViewModel>));
            Locator.CurrentMutable.Register(() => new SaveSlotFaceView(), typeof(IViewFor<SaveSlotFaceViewModel>));

            Log.Information("MHWAppearanceEditor v" + Assembly.GetExecutingAssembly().GetName().Version);
            Log.Information("Cirilla.Core v" + Assembly.GetAssembly(typeof(Cirilla.Core.Models.GMD)).GetName().Version);

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
            var logger = new LoggerConfiguration()
                .WriteTo.Sink(LogSink.AppLogger)
                .CreateLogger();
            Log.Logger = logger;
        }
    }
}
