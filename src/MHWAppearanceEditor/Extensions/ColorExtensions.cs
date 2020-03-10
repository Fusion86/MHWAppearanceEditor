using MediaColor = Avalonia.Media.Color;
using SDColor = System.Drawing.Color;

namespace MHWAppearanceEditor.Extensions
{
    public static class ColorExtensions
    {
        public static MediaColor ToMediaColor(this SDColor color) => MediaColor.FromArgb(color.A, color.R, color.G, color.B);

        public static SDColor ToSDColor(this MediaColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);
    }
}
