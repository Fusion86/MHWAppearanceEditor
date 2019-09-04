using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.Controls
{
    // This code is crap, but at least it finally works.
    public class SkinColorEdit : UserControl
    {
        private Button ColorButton => this.FindControl<Button>("ColorButton");
        private Popup SkinColorPickerPopup => this.FindControl<Popup>("SkinColorPickerPopup");
        private Thumb ColorThumb => this.FindControl<Thumb>("ColorThumb");
        private Canvas ColorCanvas => this.FindControl<Canvas>("ColorCanvas");
        private NumericUpDown NumericSkinColorX => this.FindControl<NumericUpDown>("NumericSkinColorX");
        private NumericUpDown NumericSkinColorY => this.FindControl<NumericUpDown>("NumericSkinColorY");

        private SixLabors.ImageSharp.Image<Rgba32> skinImage;

        public SkinColorEdit()
        {
            DataContext = this;
            InitializeComponent();

            // TODO:
            var imgSource = @"L:\Repos\MHWAppearanceEditor\MHWAppearanceEditor\bin\Debug\net461\assets\skin_color.png";

            ColorButton.Click += ColorEdit_Click;
            ColorCanvas.PointerPressed += ColorCanvas_PointerPressed;
            ColorCanvas.PointerMoved += ColorCanvas_PointerMoved;
            ColorThumb.DragDelta += ColorThumb_DragDelta;

            NumericSkinColorX.ValueChanged += (sender, e) => { SkinColorX = (byte)e.NewValue; };
            NumericSkinColorY.ValueChanged += (sender, e) => { SkinColorY = (byte)e.NewValue; };

            // Initial values
            Task.Run(() =>
            {
                var image = new Avalonia.Media.Imaging.Bitmap(imgSource);
                Dispatcher.UIThread.InvokeAsync(() => ColorCanvas.Background = new ImageBrush(image));

                // Backend bitmap
                skinImage = SixLabors.ImageSharp.Image.Load(imgSource);
                //UpdatePreviewColor();
            });
        }

        public static readonly StyledProperty<Color> PreviewColorProperty =
            AvaloniaProperty.Register<SkinColorEdit, Color>(nameof(PreviewColor));

        public static readonly StyledProperty<byte> SkinColorXProperty =
            AvaloniaProperty.Register<SkinColorEdit, byte>(nameof(SkinColorX));

        public static readonly StyledProperty<byte> SkinColorYProperty =
            AvaloniaProperty.Register<SkinColorEdit, byte>(nameof(SkinColorY));

        public Color PreviewColor
        {
            get => GetValue(PreviewColorProperty);
            set => SetValue(PreviewColorProperty, value);
        }

        public byte SkinColorX
        {
            get => GetValue(SkinColorXProperty);
            set
            {
                SetValue(SkinColorXProperty, value);
                UpdatePreviewColor();
                MoveThumb(value, SkinColorY);
            }
        }

        public byte SkinColorY
        {
            get => GetValue(SkinColorYProperty);
            set
            {
                SetValue(SkinColorYProperty, value);
                UpdatePreviewColor();
                MoveThumb(SkinColorX, value);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private double Clamp(double val, double min, double max)
        {
            return Math.Min(Math.Max(val, min), max);
        }

        private void MoveThumb(double x, double y)
        {
            double left = Clamp(x, 0, ColorCanvas.Bounds.Width);
            double top = Clamp(y, 0, ColorCanvas.Bounds.Height);
            Canvas.SetLeft(ColorThumb, left);
            Canvas.SetTop(ColorThumb, top);
            UpdatePreviewColor();
        }

        private void UpdatePreviewColor()
        {
            var color = skinImage[SkinColorX, SkinColorY];
            PreviewColor = new Color(color.A, color.R, color.G, color.B);
        }

        private void ColorCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.InputModifiers.HasFlag(InputModifiers.LeftMouseButton))
            {
                var position = e.GetPosition(ColorCanvas);
                SetSkinColor(position.X, position.Y);
            }
        }

        private void ColorCanvas_PointerMoved(object sender, PointerEventArgs e)
        {
            if (e.Pointer.Captured == ColorCanvas)
            {
                var position = e.GetPosition(ColorCanvas);
                SetSkinColor(position.X, position.Y);
            }
        }

        private void ColorThumb_DragDelta(object sender, VectorEventArgs e)
        {
            // TODO:
            double left = Canvas.GetLeft(ColorThumb);
            double top = Canvas.GetTop(ColorThumb);
            //SetSkinColor(left, top);
        }

        private void SetSkinColor(double x, double y)
        {
            SkinColorX = (byte)Clamp(x, 0, 255);
            SkinColorY = (byte)Clamp(y, 0, 255);
            UpdatePreviewColor();
            MoveThumb(SkinColorX, SkinColorY);
        }

        private void ColorEdit_Click(object sender, RoutedEventArgs e)
        {
            SkinColorPickerPopup.Open();
            MoveThumb(SkinColorX, SkinColorY);
        }
    }
}
