using Cirilla.Core.Enums;
using Cirilla.Core.Models;
using MHWAppearanceEditor.Interfaces;
using System.Collections.Generic;

namespace MHWAppearanceEditor.ViewModels.SaveSlotEditors
{
    public class SaveSlotInfoViewModel : ViewModelBase
    {
        public string Title => "Basic Info";

        public string HunterName { get => SaveSlot.HunterName; set => SaveSlot.HunterName = value; }
        public string PalicoName { get => SaveSlot.PalicoName; set => SaveSlot.PalicoName = value; }
        public int HunterRank { get => SaveSlot.HunterRank; set => SaveSlot.HunterRank = value; }
        public int HunterXp { get => SaveSlot.HunterXp; set => SaveSlot.HunterXp = value; }
        public int Zenny { get => SaveSlot.Zenny; set => SaveSlot.Zenny = value; }
        public int ResearchPoints { get => SaveSlot.ResearchPoints; set => SaveSlot.ResearchPoints = value; }
        public Gender Gender { get => SaveSlot.CharacterAppearance.Gender; set => SaveSlot.CharacterAppearance.Gender = value; }

        public List<Gender> Genders { get; } = new List<Gender>() { Gender.Male, Gender.Female };

        private readonly SaveSlot SaveSlot;

        public SaveSlotInfoViewModel(SaveSlot saveSlot)
        {
            SaveSlot = saveSlot;
        }
    }
}
