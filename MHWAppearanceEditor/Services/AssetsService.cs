using Avalonia.Media;
using MHWAppearanceEditor.Helpers;
using MHWAppearanceEditor.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MHWAppearanceEditor.Services
{
    public class AssetsService
    {
        private static readonly ILogger CtxLog = Log.ForContext<AssetsService>();

        public CharacterAssets CharacterAssets { get; private set; }

        public IReadOnlyCollection<Color> ColorPaletteVibrant { get; private set; }
        public IReadOnlyCollection<Color> ColorPaletteNatural { get; private set; }
        public IReadOnlyCollection<Color> FullColorPalette { get; private set; }

        public string AssetsRoot { get; }
        public string SkinColorPath { get; }

        private List<Color> colorPalette;

        public AssetsService(string root)
        {
            AssetsRoot = root;
            SkinColorPath = Path.Combine(AssetsRoot, "skin_color.png");
        }

        public void Initialize()
        {
            CharacterAssets = CharacterAssets.CreateFromAssetsMap(AssetsRoot);
            LoadPalette(Path.Combine(AssetsRoot, "palette.json"));
            CtxLog.Information("Asset loading finished");
        }

        private void LoadPalette(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                var colors = JsonConvert.DeserializeObject<List<string>>(json);
                colorPalette = colors.Select(x => Utility.ColorFromHex(x)).ToList();

                FullColorPalette = colorPalette.AsReadOnly();
                ColorPaletteVibrant = colorPalette.Take(40).ToList().AsReadOnly();
                ColorPaletteNatural = colorPalette.Skip(40).ToList().AsReadOnly();
            }
            catch (Exception ex)
            {
                CtxLog.Error(ex, ex.Message);
            }

            CtxLog.Information($"Loaded palette containing {FullColorPalette.Count} colors");
        }
    }
}
