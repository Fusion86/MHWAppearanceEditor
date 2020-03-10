using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MHWAppearanceEditor.Views.Components
{
    public class CharacterAssetView : UserControl
    {
        public CharacterAssetView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
