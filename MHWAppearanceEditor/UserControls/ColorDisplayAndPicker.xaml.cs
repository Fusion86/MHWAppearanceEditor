using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SDColor = System.Drawing.Color;

namespace MHWAppearanceEditor.UserControls
{
    /// <summary>
    /// Interaction logic for ColorDisplayAndPicker.xaml
    /// </summary>
    public partial class ColorDisplayAndPicker : UserControl
    {
        public ColorDisplayAndPicker()
        {
            InitializeComponent();
        }

        public SDColor SelectedColor
        {
            get => (SDColor)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(SDColor), typeof(ColorDisplayAndPicker), new PropertyMetadata(OnSelectedColorCallback));

        private static void OnSelectedColorCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorDisplayAndPicker o)
                o.OnSelectedColorChanged();
        }

        private void RectColor_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("// TODO: Add color picker");
        }
        
        private void OnSelectedColorChanged()
        {
            rectColor.Fill = new SolidColorBrush(Color.FromArgb(SelectedColor.A, SelectedColor.R, SelectedColor.G, SelectedColor.B));
        }
    }
}
