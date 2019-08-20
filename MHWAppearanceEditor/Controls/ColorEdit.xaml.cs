using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace MHWAppearanceEditor.Controls
{
    public class ColorEdit : UserControl
    {
        private Button ColorButton => this.FindControl<Button>("ColorButton");
        private Popup ColorPickerPopup => this.FindControl<Popup>("ColorPickerPopup");

        public ColorEdit()
        {
            DataContext = this;
            InitializeComponent();

            ColorButton.Click += ColorEdit_Click;
        }

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<ColorEdit, Color>("Color", inherits: true, defaultBindingMode: BindingMode.TwoWay);

        public Color Color
        {
            get { return GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ColorEdit_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerPopup.Open();
        }
    }
}
