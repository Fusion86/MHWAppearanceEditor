using Cirilla.Core.Models;
using System.IO;
using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Extensions;
using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;
using Xenko.Rendering;

namespace Nezuko
{
    public class HunterAppearance : SyncScript
    {
        public short HairType { get; set; }

        private NezukoApp Nezuko;
        private Material hairMaterial { get; set; }
        private Entity hairEntity;

        public override void Start()
        {
            // Alias
            Nezuko = Game as NezukoApp;

            if (Nezuko == null)
                throw new System.Exception("This SyncScript can only be used with Nezuko (Nezuko extends Xenko.Engine.Game)");

            // Load materials
            hairMaterial = Content.Load<Material>("Materials/HairMaterial");

            // Create entities with correct positions
            hairEntity = new Entity(new Vector3(0, 0, 0)) { new ModelComponent() };

            // Add to scene
            SceneSystem.SceneInstance.RootScene.Entities.Add(hairEntity);

            UpdateModels();
        }

        private string GetExtractedChunkPath(string relativePath) => Path.Combine(Nezuko.ExtractedChunksRootDir, relativePath);

        private void LoadHair(short hairType)
        {
            string str = hairType.ToString("D3"); // Pad number with leading zeroes, e.g `0 -> 000`, `13 -> 013`, `103 -> 103`
            var model = NezukoLoader.LoadMOD3(GraphicsDevice, GetExtractedChunkPath($"pl/hair/hair{str}/mod/hair{str}.mod3"));
            model.Materials.Add(hairMaterial);
            hairEntity.Get<ModelComponent>().Model = model;
        }

        public override void Update()
        {

        }

        private void UpdateModels()
        {
            LoadHair(HairType);
        }
    }
}
