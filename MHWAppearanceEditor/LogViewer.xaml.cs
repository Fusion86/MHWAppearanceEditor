using System;
using System.Threading.Tasks;
using System.Windows;

namespace MHWAppearanceEditor
{
    /// <summary>
    /// Interaction logic for LogViewer.xaml
    /// </summary>
    public partial class LogViewer : Window
    {
        private InMemorySink _sink;
        private bool readEvents = true;

        // Don't read this code
        public LogViewer(InMemorySink sink)
        {
            InitializeComponent();
            _sink = sink;
            Task.Run(() => UpdateLoop());
        }

        public async void UpdateLoop()
        {
            while (readEvents)
            {
                textBox.Dispatcher.Invoke(() => textBox.Text = string.Join(Environment.NewLine, _sink.Events));
                await Task.Delay(100);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            readEvents = false;
        }
    }
}
