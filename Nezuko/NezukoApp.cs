using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nezuko.Interfaces;
using Nezuko.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nezuko
{
    public class NezukoApp : Game
    {
        public string ExtractedChunksDir { get; set; }

        public event EventHandler<RenderTarget2D> OnDraw;

        private GraphicsDeviceManager graphics;
        private Camera camera;
        private RenderTarget2D renderTarget;
        private List<NezukoGameObject> gameObjects = new List<NezukoGameObject>();

        public NezukoApp()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            graphics.GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            camera = new Camera(GraphicsDevice, 60, 1, 1000, new Vector3(50, 50, 0), Vector3.Zero);
            CreateSizedResources();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Demo
            var loader = new NezukoLoader();
            var model = loader.LoadMod3(GraphicsDevice, @"L:\Sync\MHW Mods\chunk0\pl\hair\hair000\mod\hair000.mod3");
            var obj = new StaticModel(GraphicsDevice, model);
            gameObjects.Add(obj);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Might not have the best performance
            foreach (var obj in gameObjects.OfType<INZUpdateable>())
                obj.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Render 3D objects
            foreach (var obj in gameObjects.OfType<INZRenderable>())
                obj.Draw(GraphicsDevice, camera);

            base.Draw(gameTime);

            OnDraw?.Invoke(this, renderTarget);
        }

        protected override void EndDraw()
        {
            // Don't call Platform.Present();
        }

        private void CreateSizedResources()
        {
            renderTarget = new RenderTarget2D(GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                SurfaceFormat.ColorSRgb,
                DepthFormat.Depth24);
        }
    }
}
