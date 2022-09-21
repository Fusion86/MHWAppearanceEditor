using MHWAppearanceEditor.ViewModels;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace MHWAppearanceEditor.Models
{
    public partial class AssetsMap
    {
        public long Timestamp { get; set; }
        public List<AssetsListDefinition> Assets { get; set; }
    }

    public partial class AssetsListDefinition
    {
        public string Name { get; set; }

        [JsonProperty("texture_name")]
        public string TextureName { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
    }

    public class CharacterAssets
    {
        public List<CharacterAssetViewModel> MaleBrowTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleBrowTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> MaleFaceTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleFaceTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> MaleNoseTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleNoseTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> MaleMouthTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleMouthTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> MaleHairTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleHairTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> MaleFacialHairTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleFacialHairTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> MaleEyebrowTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleEyebrowTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> MaleEyeTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleEyeTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> MaleClothingTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleClothingTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> MaleMakeupTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> FemaleMakeupTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> PalicoCoatTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> PalicoEarTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> PalicoEyeTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> PalicoTailTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> PalicoOutlineTypes { get; } = new List<CharacterAssetViewModel>();
        public List<CharacterAssetViewModel> PalicoPupilTypes { get; } = new List<CharacterAssetViewModel>();

        private static readonly ILogger CtxLog = Log.ForContext<CharacterAssets>();

        public void LoadAssetsMap(AssetsMap map, string imgDir)
        {
            long count = 0;
            CtxLog.Information("Loading assets map...");
            foreach (var def in map.Assets)
            {
                string name;
                List<CharacterAssetViewModel> dest;

                switch (def.Name)
                {
                    case "Male Brow Types":
                        name = "Brow Type";
                        dest = MaleBrowTypes;
                        break;
                    case "Female Brow Types":
                        name = "Brow Type";
                        dest = FemaleBrowTypes;
                        break;
                    case "Male Faces":
                        name = "Face Type";
                        dest = MaleFaceTypes;
                        break;
                    case "Female Faces":
                        name = "Face Type";
                        dest = FemaleFaceTypes;
                        break;
                    case "Male Noses":
                        name = "Nose Type";
                        dest = MaleNoseTypes;
                        break;
                    case "Female Noses":
                        name = "Nose Type";
                        dest = FemaleNoseTypes;
                        break;
                    case "Male Mouths":
                        name = "Mouth Type";
                        dest = MaleMouthTypes;
                        break;
                    case "Female Mouths":
                        name = "Mouth Type";
                        dest = FemaleMouthTypes;
                        break;
                    case "Male Hairstyles":
                        name = "Male Hairstyle";
                        dest = MaleHairTypes;
                        break;
                    case "Female Hairstyles":
                        name = "Female Hairstyle";
                        dest = FemaleHairTypes;
                        break;
                    case "Male Facial Hair":
                        name = "Facial Hair Type";
                        dest = MaleFacialHairTypes;
                        break;
                    case "Female Facial Hair":
                        name = "Facial Hair Type";
                        dest = FemaleFacialHairTypes;
                        break;
                    case "Male Eyebrows":
                        name = "Eyebrow Type";
                        dest = MaleEyebrowTypes;
                        break;
                    case "Female Eyebrows":
                        name = "Eyebrow Type";
                        dest = FemaleEyebrowTypes;
                        break;
                    case "Male Eyes":
                        name = "Eye Type";
                        dest = MaleEyeTypes;
                        break;
                    case "Female Eyes":
                        name = "Eye Type";
                        dest = FemaleEyeTypes;
                        break;
                    case "Male Clothing":
                        name = "Clothing Type";
                        dest = MaleClothingTypes;
                        break;
                    case "Female Clothing":
                        name = "Clothing Type";
                        dest = FemaleClothingTypes;
                        break;
                    case "Male Makeup":
                        name = "Makeup Type";
                        dest = MaleMakeupTypes;
                        break;
                    case "Female Makeup":
                        name = "Makeup Type";
                        dest = FemaleMakeupTypes;
                        break;
                    case "Palico Coat Types":
                        name = "Coat Type";
                        dest = PalicoCoatTypes;
                        break;
                    case "Palico Ears":
                        name = "Ear Type";
                        dest = PalicoEarTypes;
                        break;
                    case "Palico Eyes":
                        name = "Eye Type";
                        dest = PalicoEyeTypes;
                        break;
                    case "Palico Tails":
                        name = "Tail Type";
                        dest = PalicoTailTypes;
                        break;
                    case "Palico Outlines":
                        name = "Outline Type";
                        dest = PalicoOutlineTypes;
                        break;
                    case "Palico Pupils":
                        name = "Pupil Type";
                        dest = PalicoPupilTypes;
                        break;
                    default:
                        CtxLog.Warning($"Unrecognized name '{def.Name}'");
                        continue;
                }

                for (int i = 0; i < def.Count; i++)
                {
                    string src = Path.Combine(imgDir, $"{def.TextureName}_{i}.png");
                    src = Path.GetFullPath(src);
                    dest.Add(new CharacterAssetViewModel($"{name} {i + 1}", src, def.Offset + i));
                    count++;
                }

                CtxLog.Verbose($"Loaded {dest.Count} items from '{def.Name}'");
            }
            CtxLog.Information($"Loaded assets map containing {count} items");
        }

        public static CharacterAssets? CreateFromAssetsMap(string assetsDir)
        {
            try
            {
                string assetsJsonPath = Path.Combine(assetsDir, "character_assets.json");
                string imgDir = Path.Combine(assetsDir, "character_assets");
                string json = File.ReadAllText(assetsJsonPath);
                var map = JsonConvert.DeserializeObject<AssetsMap>(json)!;
                var characterAssets = new CharacterAssets();
                characterAssets.LoadAssetsMap(map, imgDir);
                return characterAssets;
            }
            catch (Exception ex)
            {
                CtxLog.Error(ex, ex.Message);
                return null;
            }
        }
    }
}
