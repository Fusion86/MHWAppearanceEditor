using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace MHWAppearanceEditor.Controls
{
    public class SkinColorEdit : UserControl
    {
        private Button ColorButton => this.FindControl<Button>("ColorButton");
        private Popup SkinColorPickerPopup => this.FindControl<Popup>("SkinColorPickerPopup");

        public SkinColorEdit()
        {
            DataContext = this;
            InitializeComponent();

            ColorButton.Click += ColorEdit_Click;
        }

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<SkinColorEdit, Color>("Color", inherits: true, defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<byte> SkinColorXProperty =
            AvaloniaProperty.Register<SkinColorEdit, byte>("SkinColorX", inherits: true, defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<byte> SkinColorYProperty =
            AvaloniaProperty.Register<SkinColorEdit, byte>("SkinColorY", inherits: true, defaultBindingMode: BindingMode.TwoWay);

        public Color Color
        {
            get { return GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public byte SkinColorX
        {
            get { return GetValue(SkinColorXProperty); }
            set { SetValue(SkinColorXProperty, value); }
        }

        public byte SkinColorY
        {
            get { return GetValue(SkinColorYProperty); }
            set { SetValue(SkinColorYProperty, value); }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ColorEdit_Click(object sender, RoutedEventArgs e)
        {
            SkinColorPickerPopup.Open();
        }
    }
}
