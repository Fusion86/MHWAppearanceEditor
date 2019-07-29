using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditorNext.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MHWAppearanceEditorNext.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public static MainWindow Instance { get; private set; }

        private TabControl MainTabControl => this.FindControl<TabControl>("MainTabControl");

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();

#if DEBUG
            this.AttachDevTools();
#endif

            DataContext = new MainWindowViewModel();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.SelectedTab, v => v.MainTabControl.SelectedItem).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.OpenTabsBinding, v => v.MainTabControl.Items).DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
