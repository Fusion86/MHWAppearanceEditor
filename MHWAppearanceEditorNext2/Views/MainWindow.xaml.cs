using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditorNext2.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MHWAppearanceEditorNext2.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private TabControl MainTabControl => this.FindControl<TabControl>("MainTabControl");

        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            this.AttachDevTools();
#endif

            ViewModel = new MainWindowViewModel();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.OpenTabsBinding, v => v.MainTabControl.Items).DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
