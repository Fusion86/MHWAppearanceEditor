using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace MHWAppearanceEditor.Helpers
{
    public static class Utility
    {
        public static Window GetMainWindow()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktop)
                return classicDesktop.MainWindow;
            return null;
        }
    }
}
