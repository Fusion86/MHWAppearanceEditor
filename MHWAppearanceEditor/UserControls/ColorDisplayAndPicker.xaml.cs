using MHWAppearanceEditor.Extensions;
using MHWAppearanceEditor.Popup;
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
            DependencyProperty.Register("SelectedColor", typeof(SDColor), typeof(ColorDisplayAndPicker),
                new FrameworkPropertyMetadata(SDColor.FromArgb(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorCallback));

        private static void OnSelectedColorCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorDisplayAndPicker o)
                o.OnSelectedColorChanged();
        }

        private new void MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ColorPickerPopup popup = new ColorPickerPopup(SelectedColor.ToMediaColor());
            popup.OnColorChanged += Popup_OnColorChanged;
            popup.Show();
        }

        private void Popup_OnColorChanged(object sender, Color e)
        {
            SelectedColor = e.ToSDColor();
        }

        private void OnSelectedColorChanged()
        {
            rectColor.Fill = new SolidColorBrush(SelectedColor.ToMediaColor());
        }
    }
}
