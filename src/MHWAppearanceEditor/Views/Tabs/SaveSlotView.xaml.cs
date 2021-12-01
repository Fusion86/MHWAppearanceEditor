using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels.Tabs;
using MHWAppearanceEditor.Views.SaveSlotEditors;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MHWAppearanceEditor.Views.Tabs
{
    public class SaveSlotView : ReactiveUserControl<SaveSlotViewModel>
    {
        private SaveSlotToolsView SaveSlotToolsView => this.FindControl<SaveSlotToolsView>("SaveSlotToolsView");

        public SaveSlotView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                // Setting the datacontext in xaml doesn't work:
                // DataContext="{Binding SaveSlotToolsViewModel}
                // But binding does work. So whatever.
                this.Bind(ViewModel, vm => vm.SaveSlotToolsViewModel, v => v.SaveSlotToolsView.DataContext).DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
