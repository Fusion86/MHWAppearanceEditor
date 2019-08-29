using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SkiaSharp;
using System;

namespace MHWAppearanceEditor.Controls
{
    public class SkinColorPicker : UserControl
    {
        // Based on https://github.com/wieslawsoltes/ThemeEditor/blob/master/src/ThemeEditor.Controls.ColorPicker/ColorPicker.cs

        private Thumb ColorThumb => this.FindControl<Thumb>("ColorThumb");
        private Canvas ColorCanvas => this.FindControl<Canvas>("ColorCanvas");

        public SkinColorPicker()
        {
            InitializeComponent();

            ColorCanvas.PointerPressed += ColorCanvas_PointerPressed;
            ColorCanvas.PointerMoved += ColorCanvas_PointerMoved;
            ColorThumb.DragDelta += ColorThumb_DragDelta;
        }

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<SkinColorPicker, Color>("Color", inherits: true, defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<byte> SkinColorXProperty =
            AvaloniaProperty.Register<SkinColorPicker, byte>("SkinColorX", inherits: true, defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<byte> SkinColorYProperty =
            AvaloniaProperty.Register<SkinColorPicker, byte>("SkinColorY", inherits: true, defaultBindingMode: BindingMode.TwoWay);

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

        private double Clamp(double val, double min, double max)
        {
            return Math.Min(Math.Max(val, min), max);
        }

        private void MoveThumb(Canvas canvas, Thumb thumb, double x, double y)
        {
            double left = Clamp(x, 0, canvas.Bounds.Width);
            double top = Clamp(y, 0, canvas.Bounds.Height);
            Canvas.SetLeft(thumb, left);
            Canvas.SetTop(thumb, top);
            UpdateColor();
        }

        private void UpdateColor()
        {
            // TODO:
        }

        private void ColorCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.InputModifiers.HasFlag(InputModifiers.LeftMouseButton))
            {
                var position = e.GetPosition(ColorCanvas);
                //_updating = true;
                MoveThumb(ColorCanvas, ColorThumb, position.X, position.Y);
                //UpdateValuesFromThumbs();
                //UpdateColorFromThumbs();
                //_updating = false;
                //e.Device.Capture(ColorCanvas);
            }
        }

        private void ColorCanvas_PointerMoved(object sender, PointerEventArgs e)
        {
            if (e.Pointer.Captured == ColorCanvas)
            {
                var position = e.GetPosition(ColorCanvas);
                //_updating = true;
                MoveThumb(ColorCanvas, ColorThumb, position.X, position.Y);
                //UpdateValuesFromThumbs();
                //UpdateColorFromThumbs();
                //_updating = false;
            }
        }

        private void ColorThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(ColorThumb);
            double top = Canvas.GetTop(ColorThumb);
            MoveThumb(ColorCanvas, ColorThumb, left + e.Vector.X, top + e.Vector.Y);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
