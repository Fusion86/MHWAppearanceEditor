using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MHWAppearanceEditor.Views
{
    public class StartScreenView : ReactiveUserControl<StartScreenViewModel>
    {
        private ItemsControl SteamAccounts => this.FindControl<ItemsControl>("SteamAccounts");

        public StartScreenView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.SteamAccountsBinding, v => v.SteamAccounts.Items).DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
