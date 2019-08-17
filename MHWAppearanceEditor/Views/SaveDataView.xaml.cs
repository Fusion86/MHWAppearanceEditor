using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MHWAppearanceEditor.Views
{
    public class SaveDataView : ReactiveUserControl<SaveDataViewModel>
    {
        private TabControl TabControl => this.FindControl<TabControl>("TabControl");

        public SaveDataView()
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
