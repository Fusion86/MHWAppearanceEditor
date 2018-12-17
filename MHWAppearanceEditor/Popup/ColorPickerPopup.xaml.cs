using System;
using System.Windows;
using System.Windows.Media;

namespace MHWAppearanceEditor.Popup
{
    /// <summary>
    /// Interaction logic for ColorPickerPopup.xaml
    /// </summary>
    public partial class ColorPickerPopup : Window
    {
        public event EventHandler<Color> OnColorChanged;

        public ColorPickerPopup(Color color)
        {
            InitializeComponent();

            colorCanvas.SelectedColor = color;

            MoveWindow();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);

            // Close window when the user clicks outside of it
            Close();
        }

        private void ColorCanvas_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
                OnColorChanged?.Invoke(sender, e.NewValue.Value);
        }

        // Move window topleft to mouse pointer position
        private void MoveWindow()
        {
            var mouse = GetMousePosition();
            Left = mouse.X;
            Top = mouse.Y;
        }

        public Point GetMousePosition()
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            return new Point(point.X, point.Y);
        }
    }
}
