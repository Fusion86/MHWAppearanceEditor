using Avalonia;
using Avalonia.Animation;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.Animation
{
    public class InstantPageTransition : IPageTransition
    {
        public Task Start(Visual from, Visual to, bool forward)
        {
            return Task.CompletedTask;
        }
    }
}
