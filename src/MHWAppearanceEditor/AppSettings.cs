using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MHWAppearanceEditor
{
    public class AppSettings : ReactiveObject
    {
        [JsonProperty] [Reactive] public bool ShowFirstRunMessage { get; set; } = true;
        [JsonProperty] [Reactive] public bool EnableSteamNameLookup { get; set; } = true;
        [JsonProperty] [Reactive] public bool AutoEnableSaveCheckBypass { get; set; } = false;
    }
}
