using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels.Tabs;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MHWAppearanceEditor.Views.Tabs
{
    public class SaveSlotView : ReactiveUserControl<SaveSlotViewModel>
    {
        private TabControl TabControl => this.FindControl<TabControl>("TabControl");

        public SaveSlotView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.TabsBinding, v => v.TabControl.Items).DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
