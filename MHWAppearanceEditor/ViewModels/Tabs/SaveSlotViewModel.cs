using Cirilla.Core.Enums;
using Cirilla.Core.Models;
using MHWAppearanceEditor.Interfaces;
using MHWAppearanceEditor.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using SDColor = System.Drawing.Color;

namespace MHWAppearanceEditor.ViewModels.Tabs
{
    public class SaveSlotViewModel : ViewModelBase, ITabItemViewModel
    {
        private static readonly ILogger CtxLog = Log.ForContext<SaveSlotViewModel>();

        public string Title { [ObservableAsProperty]get; }
        public string ToolTipText => $"{HunterName} (Rank: {HunterRank})";

        // Actual SaveSlot values
        public string HunterName { get => SaveSlot.HunterName; set { SaveSlot.HunterName = value; this.RaisePropertyChanged(); } }
        public string PalicoName { get => SaveSlot.PalicoName; set => SaveSlot.PalicoName = value; }
        public int HunterRank { get => SaveSlot.HunterRank; set => SaveSlot.HunterRank = value; }
        public int HunterXp { get => SaveSlot.HunterXp; set => SaveSlot.HunterXp = value; }
        public int Zenny { get => SaveSlot.Zenny; set => SaveSlot.Zenny = value; }
        public int ResearchPoints { get => SaveSlot.ResearchPoints; set => SaveSlot.ResearchPoints = value; }
        public byte NoseHeight { get => SaveSlot.CharacterAppearance.NoseHeight; set => SaveSlot.CharacterAppearance.NoseHeight = value; }
        public byte MouthHeight { get => SaveSlot.CharacterAppearance.MouthHeight; set => SaveSlot.CharacterAppearance.MouthHeight = value; }
        public SDColor HairColor { get => SaveSlot.CharacterAppearance.HairColor; set => SaveSlot.CharacterAppearance.HairColor = value; }
        public SDColor FacialHairColor { get => SaveSlot.CharacterAppearance.FacialHairColor; set => SaveSlot.CharacterAppearance.FacialHairColor = value; }

        public Gender Gender
        {
            get => SaveSlot.CharacterAppearance.Gender;
            set { SaveSlot.CharacterAppearance.Gender = value; this.RaisePropertyChanged(); }
        }

        // Proxy properties
        public CharacterAsset BrowType
        {
            get => BrowTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.BrowType);
            set { SaveSlot.CharacterAppearance.BrowType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset FaceType
        {
            get => FaceTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.FaceType);
            set { SaveSlot.CharacterAppearance.FaceType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset NoseType
        {
            get => NoseTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.NoseType);
            set { SaveSlot.CharacterAppearance.NoseType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset MouthType
        {
            get => MouthTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.MouthType);
            set { SaveSlot.CharacterAppearance.MouthType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset HairType
        {
            get => HairTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.HairType);
            set { SaveSlot.CharacterAppearance.HairType = (short)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset FacialHairType
        {
            get => FacialHairTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.FacialHairType);
            set { SaveSlot.CharacterAppearance.FacialHairType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        // Collections of possible values
        public List<Gender> Genders { get; } = new List<Gender>() { Gender.Male, Gender.Female };
        [Reactive] public List<CharacterAsset> BrowTypes { get; private set; }
        [Reactive] public List<CharacterAsset> FaceTypes { get; private set; }
        [Reactive] public List<CharacterAsset> NoseTypes { get; private set; }
        [Reactive] public List<CharacterAsset> MouthTypes { get; private set; }
        [Reactive] public List<CharacterAsset> HairTypes { get; private set; }
        [Reactive] public List<CharacterAsset> FacialHairTypes { get; private set; }

        private readonly SaveSlot SaveSlot;
        private readonly CharacterAssets CharacterAssets;

        public SaveSlotViewModel(SaveSlot saveSlot)
        {
            CharacterAssets = MainWindowViewModel.Instance.CharacterAssets;
            SaveSlot = saveSlot;

            this.WhenAnyValue(x => x.HunterName, name => string.IsNullOrEmpty(name) ? "(blank)" : name).ToPropertyEx(this, x => x.Title);
            this.WhenAnyValue(x => x.Gender).Subscribe(UpdateGenderSpecificBindings);
        }

        private void UpdateGenderSpecificBindings(Gender gender)
        {
            try
            {
                switch (gender)
                {
                    case Gender.Male:
                        BrowTypes = CharacterAssets.MaleBrowTypes;
                        FaceTypes = CharacterAssets.MaleFaceTypes;
                        NoseTypes = CharacterAssets.MaleNoseTypes;
                        MouthTypes = CharacterAssets.MaleMouthTypes;
                        // You can use both male and female hairstyles. We display the recommended hairstyles first, in this case the __male__ hairstyles
                        HairTypes = CharacterAssets.MaleHairTypes.Concat(CharacterAssets.FemaleHairTypes).ToList();
                        FacialHairTypes = CharacterAssets.MaleFacialHairTypes;
                        break;
                    case Gender.Female:
                        BrowTypes = CharacterAssets.FemaleBrowTypes;
                        FaceTypes = CharacterAssets.FemaleFaceTypes;
                        NoseTypes = CharacterAssets.FemaleNoseTypes;
                        MouthTypes = CharacterAssets.FemaleMouthTypes;
                        // You can use both male and female hairstyles. We display the recommended hairstyles first, in this case the __female__ hairstyles
                        HairTypes = CharacterAssets.FemaleHairTypes.Concat(CharacterAssets.MaleHairTypes).ToList();
                        FacialHairTypes = CharacterAssets.FemaleFacialHairTypes;
                        break;
                }
            }
            catch (Exception ex)
            {
                CtxLog.Error(ex.Message);
            }
        }
    }
}
