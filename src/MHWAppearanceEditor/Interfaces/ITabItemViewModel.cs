using ReactiveUI;

namespace MHWAppearanceEditor.Interfaces
{
    public interface ITabItemViewModel : IReactiveObject
    {
        string Title { get; }
        string ToolTipText { get; }
    }
}
