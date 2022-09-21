using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Cirilla.Core.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

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

        public static double Lerp(double min, double max, double value)
        {
            return (1.0f - value) * min + max * value;
        }

        public static double InvLerp(double min, double max, double value)
        {
            return (value - min) / (max - min);
        }

        // I think this is called a linear conversion, though I'm not sure.
        public static double Remap(double inMin, double inMax, double outMin, double outMax, double v)
        {
            double t = InvLerp(inMin, inMax, v);
            return Lerp(outMin, outMax, t);
        }

        public static double MakeupPosToRaw(double value)
        {
            if (value <= 50) return Remap(0, 50, 1, 0, value);
            if (value <= 100) return Remap(50, 100, 0, -0.35, value);
            throw new ArgumentOutOfRangeException();
        }

        public static double MakeupPosFromRaw(double value)
        {
            //if (value == 0) return 50;
            if (value < 0 && value >= -0.35) return Remap(0, -0.35, 50, 100, value);
            if (value <= 1 && value >= 0) return Remap(1, 0, 0, 50, value);
            throw new ArgumentOutOfRangeException();
        }

        public static void OpenBrowser(string url)
        {
#if NETFRAMEWORK
            Process.Start(url);
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
#endif
        }
    }
}
