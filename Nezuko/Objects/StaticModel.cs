using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nezuko.Graphics;
using Nezuko.Interfaces;

namespace Nezuko.Objects
{
    public class StaticModel : NezukoGameObject, INZRenderable, INZUpdateable
    {
        public ModelData Model { get; }

        private BasicEffect tmpEffect;

        public StaticModel(GraphicsDevice graphicsDevice, ModelData model)
        {
            Model = model;
            tmpEffect = new BasicEffect(graphicsDevice);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GraphicsDevice graphicsDevice, ICamera camera)
        {
            tmpEffect.View = camera.ViewMatrix;
            tmpEffect.Projection = camera.ProjectionMatrix;

            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.Parts)
                {
                    graphicsDevice.SetVertexBuffer(part.VertexBuffer);
                    graphicsDevice.Indices = part.IndexBuffer;

                    foreach (var pass in tmpEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, part.VertexCount / 3);
                    }
                }
            }
        }
    }
}
