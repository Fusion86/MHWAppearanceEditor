using MHWAppearanceEditor.Popup;
using MHWAppearanceEditor.Windows;
using Serilog;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MHWAppearanceEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly InMemorySink _sink;
        private bool _isOpen;

        private static System.Action EmptyDelegate = delegate () { };

        public MainWindow()
        {
            InitializeComponent();

            // Setup logging
            _sink = new InMemorySink(vm);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Sink(_sink)
                .CreateLogger();

            Log.Information("MHWAppearanceEditor v" + Assembly.GetExecutingAssembly().GetName().Version);
            Log.Information("Cirilla.Core v" + Assembly.GetAssembly(typeof(Cirilla.Core.Models.GMD)).GetName().Version);

            _isOpen = true;

            // Horrible hack to keep the cursor blinking, without this code it won't blink (until something else forces a redraw, e.g hover effect, window resize or typing)
            Task.Run(() =>
            {
                while (_isOpen)
                {
                    codeEditor.Dispatcher.Invoke(() => { codeEditor.TextArea.UpdateLayout(); });
                    Thread.Sleep(100);
                }
            });
        }

        private void ShowLog_Click(object sender, RoutedEventArgs e)
        {
            LogViewer window = new LogViewer(_sink);
            window.Show();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vm.IsSaving)
            {
                e.Cancel = true;
                StillSavingPopup popup = new StillSavingPopup();
                popup.Owner = this;
                popup.Show();

                while (vm.IsSaving)
                    await Task.Delay(100);

                popup.Close();
                Close(); // Exit
            }

            _isOpen = false;
        }
    }
}
