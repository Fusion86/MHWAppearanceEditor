using Cirilla.Core.Models;
using MHWAppearanceEditor.Interfaces;
using MHWAppearanceEditor.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Globalization;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MHWAppearanceEditor.ViewModels.Tabs
{
    public class SaveDataInfoViewModel : ViewModelBase, ITabItemViewModel
    {
        public string Title => "SaveData Info";
        public string ToolTipText => "Global information, this is shared between all SaveSlots in this file.";

        public string FilePath => SaveData.Filepath;
        public long SteamId { get => SaveData.SteamId; set { SaveData.SteamId = value; this.RaisePropertyChanged(); } }
        //public string Checksum => "todo";
        //public string GeneratedChecksum => "todo";
        [Reactive] public string SteamIdName { get; set; }

        private readonly SaveData SaveData;
        private readonly SteamWebApiService SteamWebApi = Locator.Current.GetService<SteamWebApiService>();

        public SaveDataInfoViewModel(SaveData saveData)
        {
            SaveData = saveData;

            this.WhenAnyValue(x => x.SteamId)
                .Throttle(TimeSpan.FromSeconds(1))
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Subscribe(async x => SteamIdName = await GetPersonaName(x));
        }

        private async Task<string> GetPersonaName(long steamId)
        {
            return await SteamWebApi.GetPersonaName(steamId.ToString());
        }
    }
}
