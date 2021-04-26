using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MHWAppearanceEditor.ViewModels;
using Nito.AsyncEx.Synchronous;
using Serilog;
using System;
using System.ComponentModel;

namespace MHWAppearanceEditor.Views
{
    public class SettingsWindow : ReactiveWindow<SettingsWindowViewModel>
    {
        private static readonly ILogger log = Log.ForContext<SettingsWindow>();

        public SettingsWindow()
        {
            DataContext = new SettingsWindowViewModel();
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            try
            {
                ViewModel!.SettingsService.Save().WaitAndUnwrapException();
            }
            catch (Exception ex)
            {
                log.Error("Couldn't save settings: " + ex.Message);
            }
        }
    }
}
