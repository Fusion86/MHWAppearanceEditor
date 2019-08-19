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


    public class CharacterAsset
    {
        public string Name { get; }
        public string PreviewSource { get; }
        public int Value { get; }

        public CharacterAsset(string name, string previewSource, int value)
        {
            Name = name;
            PreviewSource = previewSource;
            Value = value;
        }
    }

    public class CharacterAssets
    {
        public List<CharacterAsset> MaleBrowTypes { get; } = new List<CharacterAsset>();
        public List<CharacterAsset> FemaleBrowTypes { get; } = new List<CharacterAsset>();
        public List<CharacterAsset> MaleFaceTypes { get; } = new List<CharacterAsset>();
        public List<CharacterAsset> FemaleFaceTypes { get; } = new List<CharacterAsset>();
        public List<CharacterAsset> MaleNoseTypes { get; } = new List<CharacterAsset>();
        public List<CharacterAsset> FemaleNoseTypes { get; } = new List<CharacterAsset>();
        public List<CharacterAsset> MaleMouthTypes { get; } = new List<CharacterAsset>();
        public List<CharacterAsset> FemaleMouthTypes { get; } = new List<CharacterAsset>();

        private static readonly ILogger CtxLog = Log.ForContext<CharacterAssets>();

        public CharacterAssets(AssetsMap map, string imgDir)
        {
            long count = 0;
            CtxLog.Information("Loading assets map...");
            foreach (var def in map.Assets)
            {
                string name;
                List<CharacterAsset> dest;

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
                    default:
                        CtxLog.Warning($"Unrecognized name '{def.Name}'");
                        continue;
                }

                for (int i = 0; i < def.Count; i++)
                {
                    string src = Path.Combine(imgDir, $"{def.TextureName}_{i}.png");
                    src = Path.GetFullPath(src);
                    dest.Add(new CharacterAsset($"{name} {i + 1}", src, def.Offset + i));
                    count++;
                }

                CtxLog.Verbose($"Loaded {dest.Count} items from '{def.Name}'");
            }
            CtxLog.Information($"Loaded assets map containing {count} items");
        }

        public static CharacterAssets LoadAssetsMap(string assetsDir)
        {
            try
            {
                string assetsJsonPath = Path.Combine(assetsDir, "assets.json");
                string imgDir = Path.Combine(assetsDir, "img");
                string json = File.ReadAllText(assetsJsonPath);
                var map = JsonConvert.DeserializeObject<AssetsMap>(json);
                return new CharacterAssets(map, imgDir);
            }
            catch (Exception ex)
            {
                CtxLog.Error(ex, ex.Message);
                return null;
            }
        }
    }
}
