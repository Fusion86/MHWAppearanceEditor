using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MHWAppearanceEditorNext
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Toggle maximized state on doubleclick titlebar
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Maximized;
            }

            DragMove();
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            // Hacky way to reimplement functionality to unmaximize the window when the user drags the titlebar
            // TODO: Only unmaximize when user moved a certain distance down while holding the left mouse button

            if (e.LeftButton == MouseButtonState.Pressed && WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
