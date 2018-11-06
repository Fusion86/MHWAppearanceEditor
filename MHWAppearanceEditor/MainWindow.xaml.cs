using Serilog;
using System.Reflection;
using System.Windows;

namespace MHWAppearanceEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly InMemorySink _sink = new InMemorySink();

        public MainWindow()
        {
            InitializeComponent();

            // Setup logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Sink(_sink)
                .CreateLogger();

            Log.Information("MHWAppearanceEditor v" + Assembly.GetExecutingAssembly().GetName().Version);
            Log.Information("Cirilla.Core v" + Assembly.GetAssembly(typeof(Cirilla.Core.Models.GMD)).GetName().Version);
        }

        private void ShowLog_Click(object sender, RoutedEventArgs e)
        {
            LogViewer window = new LogViewer(_sink);
            window.Show();
        }
    }
}
