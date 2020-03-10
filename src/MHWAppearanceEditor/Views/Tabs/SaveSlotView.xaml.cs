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
                this.Bind(ViewModel, vm => vm.SaveSlot, v => v.SaveSlotToolsView.SaveSlot).DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
