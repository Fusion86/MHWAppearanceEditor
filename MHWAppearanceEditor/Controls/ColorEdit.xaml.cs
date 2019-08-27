using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;

namespace MHWAppearanceEditor.Controls
{
    public class ColorEdit : UserControl
    {
        private Button ColorButton => this.FindControl<Button>("ColorButton");
        private Popup ColorPickerPopup => this.FindControl<Popup>("ColorPickerPopup");

        public ReactiveCommand<Color, Unit> PickColorCommand { get; }

        public ColorEdit()
        {
            DataContext = this;
            InitializeComponent();

            PickColorCommand = ReactiveCommand.Create<Color>(x => Color = x);

            ColorButton.Click += ColorEdit_Click;
        }

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<ColorEdit, Color>("Color", inherits: true, defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<IEnumerable<Color>> ColorPaletteProperty =
            AvaloniaProperty.Register<ColorEdit, IEnumerable<Color>>("ColorPalette", inherits: true, defaultBindingMode: BindingMode.OneTime);

        public Color Color
        {
            get { return GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public IEnumerable<Color> ColorPalette
        {
            get { return GetValue(ColorPaletteProperty); }
            set { SetValue(ColorPaletteProperty, value); }
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
