using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditorNext2.ViewModels;
using MHWAppearanceEditorNext2.ViewModels.Tabs;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MHWAppearanceEditorNext2.Views.Tabs
{
    public class HomeTabView : ReactiveUserControl<HomeTabViewModel>
    {
        private ListBox SteamAccounts => this.FindControl<ListBox>("SteamAccounts");
        private ListBox SaveSlots => this.FindControl<ListBox>("SaveSlots");

        public HomeTabView()
        {
            InitializeComponent();

            SaveSlots.SelectionChanged += SaveSlots_SelectionChanged;

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.SelectedAccount, v => v.SteamAccounts.SelectedItem).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.SteamAccountsBinding, v => v.SteamAccounts.Items).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SelectedAccountSaveSlots, v => v.SaveSlots.Items).DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void SaveSlots_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var slotToOpen = e.AddedItems[0] as SaveSlotViewModel;
                // Unselect item
                SaveSlots.SelectedItem = null;

                if (slotToOpen != null)
                    await ViewModel.OpenSlotCommand.Execute(slotToOpen);
            }
        }
    }
}
