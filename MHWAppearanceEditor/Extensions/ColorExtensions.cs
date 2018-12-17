using MediaColor = System.Windows.Media.Color;
using SDColor = System.Drawing.Color;

namespace MHWAppearanceEditor.Extensions
{
    /// <summary>
    /// Because we need two types of color :/
    /// </summary>
    public static class ColorExtensions
    {
        public static MediaColor ToMediaColor(this SDColor color) => MediaColor.FromArgb(color.A, color.R, color.G, color.B);

        public static SDColor ToSDColor(this MediaColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);
    }
}
