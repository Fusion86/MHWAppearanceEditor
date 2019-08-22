using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System.IO;

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

        public static string GetSafeFilename(string filename)
        {
            return string.Join("", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
