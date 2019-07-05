using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MHWAppearanceEditorNext2.ViewModels.Tabs;
using MHWAppearanceEditorNext2.Views;
using MHWAppearanceEditorNext2.Views.Tabs;
using ReactiveUI;
using Splat;

namespace MHWAppearanceEditorNext2
{
    public class App : Application
    {
        public override void Initialize()
        {
            Locator.CurrentMutable.Register(() => new HomeTabView(), typeof(IViewFor<HomeTabViewModel>));
            Locator.CurrentMutable.Register(() => new SaveSlotView(), typeof(IViewFor<SaveSlotViewModel>));

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = new MainWindow();

            base.OnFrameworkInitializationCompleted();
        }
    }
}
