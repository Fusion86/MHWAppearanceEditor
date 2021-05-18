using Avalonia.Media;
using Cirilla.Core.Enums;
using Cirilla.Core.Models;
using MHWAppearanceEditor.Helpers;
using MHWAppearanceEditor.Interfaces;
using MHWAppearanceEditor.Models;
using MHWAppearanceEditor.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using SDColor = System.Drawing.Color;

namespace MHWAppearanceEditor.ViewModels.Tabs
{
    public class SaveSlotViewModel : ViewModelBase, ITabItemViewModel
    {
        private static readonly Serilog.ILogger CtxLog = Log.ForContext<SaveSlotViewModel>();

        public string Title { [ObservableAsProperty]get; } = "";
        public string ToolTipText => $"{HunterName} (Rank: {HunterRank})";

        // Actual SaveSlot values
        public string HunterName { get => SaveSlot.HunterName; set { SaveSlot.HunterName = value; this.RaisePropertyChanged(); } }
        public string PalicoName { get => SaveSlot.PalicoName; set => SaveSlot.PalicoName = value; }
        public int HunterRank { get => SaveSlot.HunterRank; set => SaveSlot.HunterRank = value; }
        public int HunterXp { get => SaveSlot.HunterXp; set => SaveSlot.HunterXp = value; }
        public int Zenny { get => SaveSlot.Zenny; set => SaveSlot.Zenny = value; }
        public int ResearchPoints { get => SaveSlot.ResearchPoints; set => SaveSlot.ResearchPoints = value; }
        public TimeSpan PlayTime { get => TimeSpan.FromSeconds(SaveSlot.PlayTime); }

        public double NoseHeight
        {
            get => Math.Round(Utility.Remap(sbyte.MinValue, sbyte.MaxValue, 0, 100, SaveSlot.CharacterAppearance.NoseHeight), 2);
            set => SaveSlot.CharacterAppearance.NoseHeight = (sbyte)(Utility.Remap(0, 100, sbyte.MinValue, sbyte.MaxValue, value));
        }

        public double MouthHeight
        {
            get => Math.Round(Utility.Remap(sbyte.MinValue, sbyte.MaxValue, 0, 100, SaveSlot.CharacterAppearance.MouthHeight), 2);
            set => SaveSlot.CharacterAppearance.MouthHeight = (sbyte)(Utility.Remap(0, 100, sbyte.MinValue, sbyte.MaxValue, value));
        }

        public double EyeWidth
        {
            get => Math.Round(Utility.Remap(sbyte.MinValue, sbyte.MaxValue, 0, 100, SaveSlot.CharacterAppearance.EyeWidth), 2);
            set => SaveSlot.CharacterAppearance.EyeWidth = (sbyte)(Utility.Remap(0, 100, sbyte.MinValue, sbyte.MaxValue, value));
        }

        public double EyeHeight
        {
            get => Math.Round(Utility.Remap(sbyte.MinValue, sbyte.MaxValue, 0, 100, SaveSlot.CharacterAppearance.EyeHeight), 2);
            set => SaveSlot.CharacterAppearance.EyeHeight = (sbyte)(Utility.Remap(0, 100, sbyte.MinValue, sbyte.MaxValue, value));
        }

        public double Age
        {
            get => Math.Round(Utility.Remap(byte.MinValue, byte.MaxValue, 0, 100, SaveSlot.CharacterAppearance.Age), 2);
            set => SaveSlot.CharacterAppearance.Age = (byte)(Utility.Remap(0, 100, byte.MinValue, byte.MaxValue, value));
        }

        public double Wrinkles
        {
            get => Math.Round(Utility.Remap(byte.MinValue, byte.MaxValue, 0, 100, SaveSlot.CharacterAppearance.Wrinkles), 2);
            set => SaveSlot.CharacterAppearance.Wrinkles = (byte)(Utility.Remap(0, 100, byte.MinValue, byte.MaxValue, value));
        }

        public byte SkinColorX { get => SaveSlot.CharacterAppearance.SkinColorX; set => SaveSlot.CharacterAppearance.SkinColorX = value; }
        public byte SkinColorY { get => SaveSlot.CharacterAppearance.SkinColorY; set => SaveSlot.CharacterAppearance.SkinColorY = value; }

        public double Makeup1PosX
        {
            get => Math.Round(Utility.Remap(0.2, -0.2, 0, 100, SaveSlot.CharacterAppearance.Makeup1PosX), 2);
            set => SaveSlot.CharacterAppearance.Makeup1PosX = (float)(Utility.Remap(0, 100, 0.2, -0.2, value));
        }

        public double Makeup1PosY
        {
            get => Math.Round(Utility.Remap(-0.06, 0.4, 0, 100, SaveSlot.CharacterAppearance.Makeup1PosY), 2);
            set => SaveSlot.CharacterAppearance.Makeup1PosY = (byte)(Utility.Remap(0, 100, -0.06, 0.4, value));
        }

        public double Makeup1SizeX
        {
            get => Math.Round(Utility.MakeupPosFromRaw(SaveSlot.CharacterAppearance.Makeup1SizeX), 2);
            set => SaveSlot.CharacterAppearance.Makeup1SizeX = (float)Utility.MakeupPosToRaw(value);
        }

        public double Makeup1SizeY
        {
            get => Math.Round(Utility.MakeupPosFromRaw(SaveSlot.CharacterAppearance.Makeup1SizeY), 2);
            set => SaveSlot.CharacterAppearance.Makeup1SizeY = (float)Utility.MakeupPosToRaw(value);
        }

        public double Makeup1Glossy
        {
            get => Math.Round(Utility.Remap(1, 0, 0, 100, SaveSlot.CharacterAppearance.Makeup1Glossy), 2);
            set => SaveSlot.CharacterAppearance.Makeup1Glossy = (float)(Utility.Remap(0, 100, 1, 0, value));
        }

        public double Makeup1Metallic
        {
            get => Math.Round(Utility.Remap(0, 1, 0, 100, SaveSlot.CharacterAppearance.Makeup1Metallic), 2);
            set => SaveSlot.CharacterAppearance.Makeup1Metallic = (float)(Utility.Remap(0, 100, 0, 1, value));
        }

        public double Makeup1Luminescent
        {
            get => Math.Round(Utility.Remap(0, 1, 0, 100, SaveSlot.CharacterAppearance.Makeup1Luminescent), 2);
            set => SaveSlot.CharacterAppearance.Makeup1Luminescent = (float)(Utility.Remap(0, 100, 0, 1, value));
        }

        public double Makeup2PosX
        {
            get => Math.Round(Utility.Remap(0.2, -0.2, 0, 100, SaveSlot.CharacterAppearance.Makeup2PosX), 2);
            set => SaveSlot.CharacterAppearance.Makeup2PosX = (float)(Utility.Remap(0, 100, 0.2, -0.2, value));
        }

        public double Makeup2PosY
        {
            get => Math.Round(Utility.Remap(-0.06, 0.4, 0, 100, SaveSlot.CharacterAppearance.Makeup2PosY), 2);
            set => SaveSlot.CharacterAppearance.Makeup2PosY = (byte)(Utility.Remap(0, 100, -0.06, 0.4, value));
        }

        public double Makeup2SizeX
        {
            get => Math.Round(Utility.MakeupPosFromRaw(SaveSlot.CharacterAppearance.Makeup2SizeX), 2);
            set => SaveSlot.CharacterAppearance.Makeup2SizeX = (float)Utility.MakeupPosToRaw(value);
        }

        public double Makeup2SizeY
        {
            get => Math.Round(Utility.MakeupPosFromRaw(SaveSlot.CharacterAppearance.Makeup2SizeY), 2);
            set => SaveSlot.CharacterAppearance.Makeup2SizeY = (float)Utility.MakeupPosToRaw(value);
        }

        public double Makeup2Glossy
        {
            get => Math.Round(Utility.Remap(1, 0, 0, 100, SaveSlot.CharacterAppearance.Makeup2Glossy), 2);
            set => SaveSlot.CharacterAppearance.Makeup2Glossy = (float)(Utility.Remap(0, 100, 1, 0, value));
        }

        public double Makeup2Metallic
        {
            get => Math.Round(Utility.Remap(0, 1, 0, 100, SaveSlot.CharacterAppearance.Makeup2Metallic), 2);
            set => SaveSlot.CharacterAppearance.Makeup2Metallic = (float)(Utility.Remap(0, 100, 0, 1, value));
        }

        public double Makeup2Luminescent
        {
            get => Math.Round(Utility.Remap(0, 1, 0, 100, SaveSlot.CharacterAppearance.Makeup2Luminescent), 2);
            set => SaveSlot.CharacterAppearance.Makeup2Luminescent = (float)(Utility.Remap(0, 100, 0, 1, value));
        }

        public double Makeup3PosX
        {
            get => Math.Round(Utility.Remap(0.2, -0.2, 0, 100, SaveSlot.CharacterAppearance.Makeup3PosX), 2);
            set => SaveSlot.CharacterAppearance.Makeup3PosX = (float)(Utility.Remap(0, 100, 0.2, -0.2, value));
        }

        public double Makeup3PosY
        {
            get => Math.Round(Utility.Remap(-0.06, 0.4, 0, 100, SaveSlot.CharacterAppearance.Makeup3PosY), 2);
            set => SaveSlot.CharacterAppearance.Makeup3PosY = (byte)(Utility.Remap(0, 100, -0.06, 0.4, value));
        }

        public double Makeup3SizeX
        {
            get => Math.Round(Utility.MakeupPosFromRaw(SaveSlot.CharacterAppearance.Makeup3SizeX), 2);
            set => SaveSlot.CharacterAppearance.Makeup3SizeX = (float)Utility.MakeupPosToRaw(value);
        }

        public double Makeup3SizeY
        {
            get => Math.Round(Utility.MakeupPosFromRaw(SaveSlot.CharacterAppearance.Makeup3SizeY), 2);
            set => SaveSlot.CharacterAppearance.Makeup3SizeY = (float)Utility.MakeupPosToRaw(value);
        }

        public double Makeup3Glossy
        {
            get => Math.Round(Utility.Remap(1, 0, 0, 100, SaveSlot.CharacterAppearance.Makeup3Glossy), 2);
            set => SaveSlot.CharacterAppearance.Makeup3Glossy = (float)(Utility.Remap(0, 100, 1, 0, value));
        }

        public double Makeup3Metallic
        {
            get => Math.Round(Utility.Remap(0, 1, 0, 100, SaveSlot.CharacterAppearance.Makeup3Metallic), 2);
            set => SaveSlot.CharacterAppearance.Makeup3Metallic = (float)(Utility.Remap(0, 100, 0, 1, value));
        }

        public double Makeup3Luminescent
        {
            get => Math.Round(Utility.Remap(0, 1, 0, 100, SaveSlot.CharacterAppearance.Makeup3Luminescent), 2);
            set => SaveSlot.CharacterAppearance.Makeup3Luminescent = (float)(Utility.Remap(0, 100, 0, 1, value));
        }

        public SDColor HairColor { get => SaveSlot.CharacterAppearance.HairColor; set => SaveSlot.CharacterAppearance.HairColor = value; }
        public SDColor FacialHairColor { get => SaveSlot.CharacterAppearance.FacialHairColor; set => SaveSlot.CharacterAppearance.FacialHairColor = value; }
        public SDColor LeftEyeColor { get => SaveSlot.CharacterAppearance.LeftEyeColor; set => SaveSlot.CharacterAppearance.LeftEyeColor = value; }
        public SDColor RightEyeColor { get => SaveSlot.CharacterAppearance.RightEyeColor; set => SaveSlot.CharacterAppearance.RightEyeColor = value; }
        public SDColor EyebrowColor { get => SaveSlot.CharacterAppearance.EyebrowColor; set => SaveSlot.CharacterAppearance.EyebrowColor = value; }
        public SDColor ClothingColor { get => SaveSlot.CharacterAppearance.ClothingColor; set => SaveSlot.CharacterAppearance.ClothingColor = value; }
        public SDColor Makeup1Color { get => SaveSlot.CharacterAppearance.Makeup1Color; set => SaveSlot.CharacterAppearance.Makeup1Color = value; }
        public SDColor Makeup2Color { get => SaveSlot.CharacterAppearance.Makeup2Color; set => SaveSlot.CharacterAppearance.Makeup2Color = value; }
        public SDColor Makeup3Color { get => SaveSlot.CharacterAppearance.Makeup3Color; set => SaveSlot.CharacterAppearance.Makeup3Color = value; }
        public EyelashLength EyelashLength { get => SaveSlot.CharacterAppearance.EyelashLength; set => SaveSlot.CharacterAppearance.EyelashLength = value; }

        public SDColor PalicoPatternColor1 { get => SaveSlot.PalicoAppearance.PatternColor1; set => SaveSlot.PalicoAppearance.PatternColor1 = value; }
        public SDColor PalicoPatternColor2 { get => SaveSlot.PalicoAppearance.PatternColor2; set => SaveSlot.PalicoAppearance.PatternColor2 = value; }
        public SDColor PalicoPatternColor3 { get => SaveSlot.PalicoAppearance.PatternColor3; set => SaveSlot.PalicoAppearance.PatternColor3 = value; }
        public SDColor PalicoFurColor { get => SaveSlot.PalicoAppearance.FurColor; set => SaveSlot.PalicoAppearance.FurColor = value; }
        public SDColor PalicoLeftEyeColor { get => SaveSlot.PalicoAppearance.LeftEyeColor; set => SaveSlot.PalicoAppearance.LeftEyeColor = value; }
        public SDColor PalicoRightEyeColor { get => SaveSlot.PalicoAppearance.RightEyeColor; set => SaveSlot.PalicoAppearance.RightEyeColor = value; }
        public SDColor PalicoClothingColor { get => SaveSlot.PalicoAppearance.ClothingColor; set => SaveSlot.PalicoAppearance.ClothingColor = value; }
        public float PalicoFurLength { get => SaveSlot.PalicoAppearance.FurLength; set => SaveSlot.PalicoAppearance.FurLength = value; }
        public float PalicoFurThickness { get => SaveSlot.PalicoAppearance.FurThickness; set => SaveSlot.PalicoAppearance.FurThickness = value; }
        public PalicoVoiceType PalicoVoiceType { get => SaveSlot.PalicoAppearance.VoiceType; set => SaveSlot.PalicoAppearance.VoiceType = value; }
        public PalicoVoicePitch PalicoVoicePitch { get => SaveSlot.PalicoAppearance.VoicePitch; set => SaveSlot.PalicoAppearance.VoicePitch = value; }

        public short HairTypeRaw { get => SaveSlot.CharacterAppearance.HairType; set => SaveSlot.CharacterAppearance.HairType = value; }

        public Gender Gender
        {
            get => SaveSlot.CharacterAppearance.Gender;
            set { SaveSlot.CharacterAppearance.Gender = value; this.RaisePropertyChanged(); }
        }

        // Proxy properties
        public CharacterAssetViewModel BrowType
        {
            get => BrowTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.BrowType);
            set { SaveSlot.CharacterAppearance.BrowType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel FaceType
        {
            get => FaceTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.FaceType);
            set { SaveSlot.CharacterAppearance.FaceType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel NoseType
        {
            get => NoseTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.NoseType);
            set { SaveSlot.CharacterAppearance.NoseType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel MouthType
        {
            get => MouthTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.MouthType);
            set { SaveSlot.CharacterAppearance.MouthType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel HairType
        {
            get => HairTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.HairType);
            set { SaveSlot.CharacterAppearance.HairType = (short)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel FacialHairType
        {
            get => FacialHairTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.FacialHairType);
            set { SaveSlot.CharacterAppearance.FacialHairType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel EyebrowType
        {
            get => EyebrowTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.EyebrowType);
            set { SaveSlot.CharacterAppearance.EyebrowType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel EyeType
        {
            get => EyeTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.EyeType);
            set { SaveSlot.CharacterAppearance.EyeType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel ClothingType
        {
            get => ClothingTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.ClothingType);
            set { SaveSlot.CharacterAppearance.ClothingType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel Makeup1Type
        {
            get => MakeupTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.Makeup1Type);
            set { SaveSlot.CharacterAppearance.Makeup1Type = value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel Makeup2Type
        {
            get => MakeupTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.Makeup2Type);
            set { SaveSlot.CharacterAppearance.Makeup2Type = value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel Makeup3Type
        {
            get => MakeupTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.Makeup3Type);
            set { SaveSlot.CharacterAppearance.Makeup3Type = value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel VoiceType
        {
            get => VoiceTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.Voice);
            set { SaveSlot.CharacterAppearance.Voice = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel ExpressionType
        {
            get => ExpressionTypes.FirstOrDefault(x => x.Value == SaveSlot.CharacterAppearance.Expression);
            set { SaveSlot.CharacterAppearance.Expression = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel PalicoPatternType
        {
            get => PalicoPatternTypes.FirstOrDefault(x => x.Value == SaveSlot.PalicoAppearance.PatternType);
            set { SaveSlot.PalicoAppearance.PatternType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel PalicoEyeType
        {
            get => PalicoEyeTypes.FirstOrDefault(x => x.Value == SaveSlot.PalicoAppearance.EyeType);
            set { SaveSlot.PalicoAppearance.EyeType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel PalicoEarType
        {
            get => PalicoEarTypes.FirstOrDefault(x => x.Value == SaveSlot.PalicoAppearance.EarType);
            set { SaveSlot.PalicoAppearance.EarType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        public CharacterAssetViewModel PalicoTailType
        {
            get => PalicoTailTypes.FirstOrDefault(x => x.Value == SaveSlot.PalicoAppearance.TailType);
            set { SaveSlot.PalicoAppearance.TailType = (byte)value.Value; this.RaisePropertyChanged(); }
        }

        // Collections of possible values
        public List<Gender> Genders { get; } = new List<Gender>() { Gender.Male, Gender.Female };
        public List<EyelashLength> EyelashLengths { get; } = new List<EyelashLength>() { EyelashLength.Short, EyelashLength.Average, EyelashLength.Long };
        public List<PalicoVoiceType> PalicoVoiceTypes { get; } = new List<PalicoVoiceType>() { PalicoVoiceType.Type1, PalicoVoiceType.Type2, PalicoVoiceType.Type3 };
        public List<PalicoVoicePitch> PalicoVoicePitches { get; } = new List<PalicoVoicePitch>() { PalicoVoicePitch.MediumPitch, PalicoVoicePitch.LowPitch, PalicoVoicePitch.HighPitch };

        [Reactive] public List<CharacterAssetViewModel> BrowTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> FaceTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> NoseTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> MouthTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> HairTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> FacialHairTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> EyebrowTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> EyeTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> ClothingTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> MakeupTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> VoiceTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> ExpressionTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> PalicoPatternTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> PalicoEyeTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> PalicoEarTypes { get; private set; }
        [Reactive] public List<CharacterAssetViewModel> PalicoTailTypes { get; private set; }

        public IReadOnlyCollection<Color> ColorPaletteVibrant { get; }
        public IReadOnlyCollection<Color> ColorPaletteNatural { get; }

        public SaveSlot SaveSlot { get; }

        private readonly CharacterAssets characterAssets;

        public SaveSlotViewModel(SaveSlot saveSlot, AssetsService? assetsService = null)
        {
            SaveSlot = saveSlot;
            assetsService ??= Locator.Current.GetService<AssetsService>()!;
            characterAssets = assetsService.CharacterAssets;
            ColorPaletteVibrant = assetsService.ColorPaletteVibrant;
            ColorPaletteNatural = assetsService.ColorPaletteNatural;

            this.WhenAnyValue(x => x.HunterName, name => string.IsNullOrEmpty(name) ? "(blank)" : name).ToPropertyEx(this, x => x.Title);
            this.WhenAnyValue(x => x.Gender).Subscribe(UpdateGenderSpecificBindings);

            try
            {
                PalicoPatternTypes = characterAssets.PalicoCoatTypes;
                PalicoEyeTypes = characterAssets.PalicoEyeTypes;
                PalicoEarTypes = characterAssets.PalicoEarTypes;
                PalicoTailTypes = characterAssets.PalicoTailTypes;
            }
            catch (Exception ex)
            {
                CtxLog.Error(ex, ex.Message);
            }

            // Hardcoded voicetypes, since we don't have a preview for those (yet)
            VoiceTypes = Enumerable.Range(0, 20).Select(x => new CharacterAssetViewModel($"Voice Type {x + 1}", null, x)).ToList();
            ExpressionTypes = Enumerable.Range(0, 5).Select(x => new CharacterAssetViewModel($"Expression Type {x + 1}", null, x)).ToList();
        }

        private void UpdateGenderSpecificBindings(Gender gender)
        {
            try
            {
                switch (gender)
                {
                    case Gender.Male:
                        BrowTypes = characterAssets.MaleBrowTypes;
                        FaceTypes = characterAssets.MaleFaceTypes;
                        NoseTypes = characterAssets.MaleNoseTypes;
                        MouthTypes = characterAssets.MaleMouthTypes;
                        // You can use both male and female hairstyles. We display the recommended hairstyles first, in this case the __male__ hairstyles
                        //HairTypes = characterAssets.MaleHairTypes.Concat(characterAssets.FemaleHairTypes).ToList();
                        FacialHairTypes = characterAssets.MaleFacialHairTypes;
                        EyebrowTypes = characterAssets.MaleEyebrowTypes;
                        EyeTypes = characterAssets.MaleEyeTypes;
                        ClothingTypes = characterAssets.MaleClothingTypes;
                        MakeupTypes = characterAssets.MaleMakeupTypes;
                        break;
                    case Gender.Female:
                        BrowTypes = characterAssets.FemaleBrowTypes;
                        FaceTypes = characterAssets.FemaleFaceTypes;
                        NoseTypes = characterAssets.FemaleNoseTypes;
                        MouthTypes = characterAssets.FemaleMouthTypes;
                        // You can use both male and female hairstyles. We display the recommended hairstyles first, in this case the __female__ hairstyles
                        //HairTypes = characterAssets.FemaleHairTypes.Concat(characterAssets.MaleHairTypes).ToList();
                        FacialHairTypes = characterAssets.FemaleFacialHairTypes;
                        EyebrowTypes = characterAssets.FemaleEyebrowTypes;
                        EyeTypes = characterAssets.FemaleEyeTypes;
                        ClothingTypes = characterAssets.FemaleClothingTypes;
                        MakeupTypes = characterAssets.FemaleMakeupTypes;
                        break;
                }
            }
            catch (Exception ex)
            {
                CtxLog.Error(ex, ex.Message);
            }
        }
    }
}
