using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MHWAppearanceEditor.Views
{
    public class HelpWindow : Window
    {
        public HelpWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
