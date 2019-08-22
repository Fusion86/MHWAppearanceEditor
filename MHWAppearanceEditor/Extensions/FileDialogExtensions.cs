using Avalonia.Controls;
using MHWAppearanceEditor.Helpers;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.Extensions
{
    public static class FileDialogExtensions
    {
        public static async Task<string[]> ShowAsync(this OpenFileDialog ofd)
        {
            return await ofd.ShowAsync(Utility.GetMainWindow());
        }

        public static async Task<string> ShowAsync(this SaveFileDialog sfd)
        {
            return await sfd.ShowAsync(Utility.GetMainWindow());
        }
    }
}
