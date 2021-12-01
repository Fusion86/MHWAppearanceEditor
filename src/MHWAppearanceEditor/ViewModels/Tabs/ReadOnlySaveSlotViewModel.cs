using Cirilla.Core.Models;
using System;

namespace MHWAppearanceEditor.ViewModels.Tabs
{
    // This is currently only used in the SaveSlotToolsViewModel.
    // We can't use the normal SaveSlotViewModel in there, because that would create a recursive dependency on itself (see SourceSaveSlots)
    public class ReadOnlySaveSlotViewModel : ViewModelBase
    {
        public string HunterName => SaveSlot.HunterName;
        public string PalicoName => SaveSlot.PalicoName;
        public int HunterRank => SaveSlot.HunterRank;
        public int HunterXp => SaveSlot.HunterXp;
        public int Zenny => SaveSlot.Zenny;
        public int ResearchPoints => SaveSlot.ResearchPoints;
        public TimeSpan PlayTime => TimeSpan.FromSeconds(SaveSlot.PlayTime);

        public SaveSlot SaveSlot { get; }

        public ReadOnlySaveSlotViewModel(SaveSlot saveSlot)
        {
            SaveSlot = saveSlot;
        }
    }
}
