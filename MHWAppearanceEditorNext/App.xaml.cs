using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MHWAppearanceEditorNext.ViewModels.Tabs;
using MHWAppearanceEditorNext.Views;
using MHWAppearanceEditorNext.Views.Tabs;
using ReactiveUI;
using Splat;

namespace MHWAppearanceEditorNext
{
    public class App : Application
    {
        public override void Initialize()
        {
            Locator.CurrentMutable.Register(() => new HomeTabView(), typeof(IViewFor<HomeViewModel>));
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
