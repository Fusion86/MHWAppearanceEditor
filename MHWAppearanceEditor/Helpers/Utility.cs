using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using System.IO;
using Cirilla.Core.Extensions;
using System;

namespace MHWAppearanceEditor.Helpers
{
    public static class Utility
    {
        public static Window GetMainWindow()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktop)
                return classicDesktop.MainWindow;
            throw new NotSupportedException();
        }

        public static string GetSafeFilename(string filename)
        {
            return string.Join("", filename.Split(Path.GetInvalidFileNameChars()));
        }

        // This only works for "long" color codes. E.g. omething like #CCC doesn't expand to #CCCCCC.
        public static Color ColorFromHex(string hex)
        {
            var b = hex.ParseHexString();

            if (b.Length == 4)
                return Color.FromArgb(b[3], b[0], b[1], b[2]);
            else if (b.Length == 3)
                return Color.FromRgb(b[0], b[1], b[2]);
            return default;
        }
    }
}
