using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;

namespace MHWAppearanceEditor.Controls
{
    public class SkinColorEdit : UserControl
    {
        private Button ColorButton => this.FindControl<Button>("ColorButton");
        private Popup SkinColorPickerPopup => this.FindControl<Popup>("SkinColorPickerPopup");
        private Thumb ColorThumb => this.FindControl<Thumb>("ColorThumb");
        private Canvas ColorCanvas => this.FindControl<Canvas>("ColorCanvas");

        public SkinColorEdit()
        {
            DataContext = this;
            InitializeComponent();

            ColorButton.Click += ColorEdit_Click;
            ColorCanvas.PointerPressed += ColorCanvas_PointerPressed;
            ColorCanvas.PointerMoved += ColorCanvas_PointerMoved;
            //ColorThumb.DragDelta += ColorThumb_DragDelta;
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

        static Random rnd = new Random();
        private void UpdatePreviewColor()
        {
            int i = rnd.Next(0, 5);

            switch (i)
            {
                case 0: PreviewColor = Colors.Red; break;
                case 1: PreviewColor = Colors.Yellow; break;
                case 2: PreviewColor = Colors.Cyan; break;
                case 3: PreviewColor = Colors.Purple; break;
                case 4: PreviewColor = Colors.Green; break;
                case 5: PreviewColor = Colors.Blue; break;
            }
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

        //private void ColorThumb_DragDelta(object sender, VectorEventArgs e)
        //{
        //    double left = Canvas.GetLeft(ColorThumb);
        //    double top = Canvas.GetTop(ColorThumb);
        //    MoveThumb(left + e.Vector.X, top + e.Vector.Y);
        //}

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
        }
    }
}
