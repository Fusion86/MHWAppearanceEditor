using Cirilla.Core.Models;
using MHWAppearanceEditor.Interfaces;

namespace MHWAppearanceEditor.ViewModels.Tabs
{
    public class SaveDataInfoViewModel : ViewModelBase, ITabItemViewModel
    {
        public string Title => "SaveData Info";
        public string ToolTipText => "Global information, this is shared between all SaveSlots in this file.";

        public string FilePath => SaveData.Filepath;
        public long SteamId => SaveData.SteamId;
        public string Checksum => "todo";
        public string GeneratedChecksum => "todo";

        private readonly SaveData SaveData;

        public SaveDataInfoViewModel(SaveData saveData)
        {
            SaveData = saveData;
        }
    }
}
