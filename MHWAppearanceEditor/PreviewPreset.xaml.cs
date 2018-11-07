using System.Diagnostics;
using System.Windows;

namespace MHWAppearanceEditor
{
    /// <summary>
    /// Interaction logic for PreviewPreset.xaml
    /// </summary>
    public partial class PreviewPreset : Window
    {
        public PreviewPreset(SerializableMetadata metaData)
        {
            InitializeComponent();

            DataContext = metaData;

            Title = $"Preset {metaData.Title} by {metaData.Author}";
        }

        private void Import(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
        }
    }
}
