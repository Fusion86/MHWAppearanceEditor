using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MHWAppearanceEditor
{
    public class AppSettings : ReactiveObject
    {
        [JsonProperty] [Reactive] public bool ShowFirstRunMessage { get; set; } = true;
        [JsonProperty] [Reactive] public bool EnableSteamNameLookup { get; set; } = true;
        [JsonProperty] [Reactive] public bool AutoEnableSaveCheckBypass { get; set; } = true;

        /// <summary>
        /// Maximum amount of backups to keep. Set to zero to disable backuping.
        /// </summary>
        [JsonProperty] [Reactive] public int MaxBackupCount { get; set; } = 10;
    }
}
