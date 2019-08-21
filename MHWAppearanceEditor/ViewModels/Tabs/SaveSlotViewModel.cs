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
        public byte EyeWidth { get => SaveSlot.CharacterAppearance.EyeWidth; set => SaveSlot.CharacterAppearance.EyeWidth = value; }
        public byte EyeHeight { get => SaveSlot.CharacterAppearance.EyeHeight; set => SaveSlot.CharacterAppearance.EyeHeight = value; }
        public float Makeup1PosX { get => SaveSlot.CharacterAppearance.Makeup1PosX; set => SaveSlot.CharacterAppearance.Makeup1PosX = value; }
        public float Makeup1PosY { get => SaveSlot.CharacterAppearance.Makeup1PosY; set => SaveSlot.CharacterAppearance.Makeup1PosY = value; }
        public float Makeup1SizeX { get => SaveSlot.CharacterAppearance.Makeup1SizeX; set => SaveSlot.CharacterAppearance.Makeup1SizeX = value; }
        public float Makeup1SizeY { get => SaveSlot.CharacterAppearance.Makeup1SizeY; set => SaveSlot.CharacterAppearance.Makeup1SizeY = value; }
        public float Makeup1Glossy { get => SaveSlot.CharacterAppearance.Makeup1Glossy; set => SaveSlot.CharacterAppearance.Makeup1Glossy = value; }
        public float Makeup1Metallic { get => SaveSlot.CharacterAppearance.Makeup1Metallic; set => SaveSlot.CharacterAppearance.Makeup1Metallic = value; }
        public float Makeup2PosX { get => SaveSlot.CharacterAppearance.Makeup2PosX; set => SaveSlot.CharacterAppearance.Makeup2PosX = value; }
        public float Makeup2PosY { get => SaveSlot.CharacterAppearance.Makeup2PosY; set => SaveSlot.CharacterAppearance.Makeup2PosY = value; }
        public float Makeup2SizeX { get => SaveSlot.CharacterAppearance.Makeup2SizeX; set => SaveSlot.CharacterAppearance.Makeup2SizeX = value; }
        public float Makeup2SizeY { get => SaveSlot.CharacterAppearance.Makeup2SizeY; set => SaveSlot.CharacterAppearance.Makeup2SizeY = value; }
        public float Makeup2Glossy { get => SaveSlot.CharacterAppearance.Makeup2Glossy; set => SaveSlot.CharacterAppearance.Makeup2Glossy = value; }
        public float Makeup2Metallic { get => SaveSlot.CharacterAppearance.Makeup2Metallic; set => SaveSlot.CharacterAppearance.Makeup2Metallic = value; }
        public SDColor HairColor { get => SaveSlot.CharacterAppearance.HairColor; set => SaveSlot.CharacterAppearance.HairColor = value; }
        public SDColor FacialHairColor { get => SaveSlot.CharacterAppearance.FacialHairColor; set => SaveSlot.CharacterAppearance.FacialHairColor = value; }
        public SDColor LeftEyeColor { get => SaveSlot.CharacterAppearance.LeftEyeColor; set => SaveSlot.CharacterAppearance.LeftEyeColor = value; }
        public SDColor RightEyeColor { get => SaveSlot.CharacterAppearance.RightEyeColor; set => SaveSlot.CharacterAppearance.RightEyeColor = value; }
        public SDColor EyebrowColor { get => SaveSlot.CharacterAppearance.EyebrowColor; set => SaveSlot.CharacterAppearance.EyebrowColor = value; }
        public SDColor ClothingColor { get => SaveSlot.CharacterAppearance.ClothingColor; set => SaveSlot.CharacterAppearance.ClothingColor = value; }
        public SDColor Makeup1Color { get => SaveSlot.CharacterAppearance.Makeup1Color; set => SaveSlot.CharacterAppearance.Makeup1Color = value; }
        public SDColor Makeup2Color { get => SaveSlot.CharacterAppearance.Makeup2Color; set => SaveSlot.CharacterAppearance.Makeup2Color = value; }
        public EyelashLength EyelashLength { get => SaveSlot.CharacterAppearance.EyelashLength; set => SaveSlot.CharacterAppearance.EyelashLength = value; }

        public SDColor PalicoPatternColor1 { get => SaveSlot.PalicoAppearance.PatternColor1; set => SaveSlot.PalicoAppearance.PatternColor1 = value; }
        public SDColor PalicoPatternColor2 { get => SaveSlot.PalicoAppearance.PatternColor2; set => SaveSlot.PalicoAppearance.PatternColor2 = value; }
        public SDColor PalicoPatternColor3 { get => SaveSlot.PalicoAppearance.PatternColor3; set => SaveSlot.PalicoAppearance.PatternColor3 = value; }
        public SDColor PalicoFurColor { get => SaveSlot.PalicoAppearance.FurColor; set => SaveSlot.PalicoAppearance.FurColor = value; }
        public SDColor PalicoLeftEyeColor { get => SaveSlot.PalicoAppearance.LeftEyeColor; set => SaveSlot.PalicoAppearance.LeftEyeColor = value; }
        public SDColor PalicoRightEyeColor { get => SaveSlot.PalicoAppearance.RightEyeColor; set => SaveSlot.PalicoAppearance.RightEyeColor = value; }
        public SDColor PalicoClothingColor { get => SaveSlot.PalicoAppearance.ClothingColor; set => SaveSlot.PalicoAppearance.ClothingColor = value; }
        public PalicoVoiceType PalicoVoiceType { get => SaveSlot.PalicoAppearance.VoiceType; set => SaveSlot.PalicoAppearance.VoiceType = value; }
        public PalicoVoicePitch PalicoVoicePitch { get => SaveSlot.PalicoAppearance.VoicePitch; set => SaveSlot.PalicoAppearance.VoicePitch = value; }

        public Gender Gender
        {
            get => SaveSlot.CharacterAppearance.Gender;
            set { SaveSlot.CharacterAppearance.Gender = value; this.RaisePropertyChanged(); }
        }

        // Proxy properties
#pragma warning disable CA1062 // Validate arguments of public methods
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

        public CharacterAsset EyebrowType
        {
            get => EyebrowTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.EyebrowType);
            set { SaveSlot.CharacterAppearance.EyebrowType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset EyeType
        {
            get => EyeTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.EyeType);
            set { SaveSlot.CharacterAppearance.EyeType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset ClothingType
        {
            get => ClothingTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.ClothingType);
            set { SaveSlot.CharacterAppearance.ClothingType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset Makeup1Type
        {
            get => MakeupTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.Makeup1Type);
            set { SaveSlot.CharacterAppearance.Makeup1Type = value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset Makeup2Type
        {
            get => MakeupTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.Makeup2Type);
            set { SaveSlot.CharacterAppearance.Makeup2Type = value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset PalicoPatternType
        {
            get => PalicoPatternTypes.FirstOrDefault(x => x.Value == SaveSlot.PalicoAppearance.PatternType);
            set { SaveSlot.PalicoAppearance.PatternType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset PalicoEyeType
        {
            get => PalicoEyeTypes.FirstOrDefault(x => x.Value == SaveSlot.PalicoAppearance.EyeType);
            set { SaveSlot.PalicoAppearance.EyeType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset PalicoEarType
        {
            get => PalicoEarTypes.FirstOrDefault(x => x.Value == SaveSlot.PalicoAppearance.EarType);
            set { SaveSlot.PalicoAppearance.EarType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAsset PalicoTailType
        {
            get => PalicoTailTypes.FirstOrDefault(x => x.Value == SaveSlot.PalicoAppearance.TailType);
            set { SaveSlot.PalicoAppearance.TailType = (byte)value.Value; this.RaisePropertyChanged(); }
        }
#pragma warning restore CA1062 // Validate arguments of public methods

        // Collections of possible values
        public List<Gender> Genders { get; } = new List<Gender>() { Gender.Male, Gender.Female };
        public List<EyelashLength> EyelashLengths { get; } = new List<EyelashLength>() { EyelashLength.Short, EyelashLength.Average, EyelashLength.Long };
        public List<PalicoVoiceType> PalicoVoiceTypes { get; } = new List<PalicoVoiceType>() { PalicoVoiceType.Type1, PalicoVoiceType.Type2, PalicoVoiceType.Type3 };
        public List<PalicoVoicePitch> PalicoVoicePitches { get; } = new List<PalicoVoicePitch>() { PalicoVoicePitch.MediumPitch, PalicoVoicePitch.LowPitch, PalicoVoicePitch.HighPitch };
        [Reactive] public List<CharacterAsset> BrowTypes { get; private set; }
        [Reactive] public List<CharacterAsset> FaceTypes { get; private set; }
        [Reactive] public List<CharacterAsset> NoseTypes { get; private set; }
        [Reactive] public List<CharacterAsset> MouthTypes { get; private set; }
        [Reactive] public List<CharacterAsset> HairTypes { get; private set; }
        [Reactive] public List<CharacterAsset> FacialHairTypes { get; private set; }
        [Reactive] public List<CharacterAsset> EyebrowTypes { get; private set; }
        [Reactive] public List<CharacterAsset> EyeTypes { get; private set; }
        [Reactive] public List<CharacterAsset> ClothingTypes { get; private set; }
        [Reactive] public List<CharacterAsset> MakeupTypes { get; private set; }
        [Reactive] public List<CharacterAsset> PalicoPatternTypes { get; private set; }
        [Reactive] public List<CharacterAsset> PalicoEyeTypes { get; private set; }
        [Reactive] public List<CharacterAsset> PalicoEarTypes { get; private set; }
        [Reactive] public List<CharacterAsset> PalicoTailTypes { get; private set; }

        public SaveSlot SaveSlot { get; }

        private readonly CharacterAssets CharacterAssets;

        public SaveSlotViewModel(SaveSlot saveSlot)
        {
            CharacterAssets = MainWindowViewModel.Instance.CharacterAssets;
            SaveSlot = saveSlot;

            PalicoPatternTypes = CharacterAssets.PalicoCoatTypes;
            PalicoEyeTypes = CharacterAssets.PalicoEyeTypes;
            PalicoEarTypes = CharacterAssets.PalicoEarTypes;
            PalicoTailTypes = CharacterAssets.PalicoTailTypes;

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
                        EyebrowTypes = CharacterAssets.MaleEyebrowTypes;
                        EyeTypes = CharacterAssets.MaleEyeTypes;
                        ClothingTypes = CharacterAssets.MaleClothingTypes;
                        MakeupTypes = CharacterAssets.MaleMakeupTypes;
                        break;
                    case Gender.Female:
                        BrowTypes = CharacterAssets.FemaleBrowTypes;
                        FaceTypes = CharacterAssets.FemaleFaceTypes;
                        NoseTypes = CharacterAssets.FemaleNoseTypes;
                        MouthTypes = CharacterAssets.FemaleMouthTypes;
                        // You can use both male and female hairstyles. We display the recommended hairstyles first, in this case the __female__ hairstyles
                        HairTypes = CharacterAssets.FemaleHairTypes.Concat(CharacterAssets.MaleHairTypes).ToList();
                        FacialHairTypes = CharacterAssets.FemaleFacialHairTypes;
                        EyebrowTypes = CharacterAssets.FemaleEyebrowTypes;
                        EyeTypes = CharacterAssets.FemaleEyeTypes;
                        ClothingTypes = CharacterAssets.FemaleClothingTypes;
                        MakeupTypes = CharacterAssets.FemaleMakeupTypes;
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
