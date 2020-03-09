using ReactiveUI.Fody.Helpers;

namespace MHWAppearanceEditor
{
    public class AppSettings
    {
        [Reactive] public bool EnableSteamNameLookup { get; set; } = true;
    }
}
