using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.Services;
using MHWAppearanceEditor.ViewModels;
using ReactiveUI;
using Splat;
using System.ComponentModel;
using System.Reactive.Disposables;

namespace MHWAppearanceEditor.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private DataGrid LogDataGrid => this.FindControl<DataGrid>("LogDataGrid");

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.EventsBinding, v => v.LogDataGrid.Items).DisposeWith(disposables);

                //ViewModel.EventsBinding.ObserveCollectionChanges().Subscribe(x =>
                //{
                //    Dispatcher.UIThread.InvokeAsync(() =>
                //    {
                //        // HACK: Might break on different DPI displays
                //        double height = ViewModel.EventsBinding.Count * 24 + 26;
                //        LogDataGrid.Height = Math.Min(height, 266);
                //    });
                //}).DisposeWith(disposables);
            });
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Locator.Current.GetService<OdogaronService>()?.Stop();
            base.OnClosing(e);
        }
    }
}
