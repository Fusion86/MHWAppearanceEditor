using Microsoft.Xna.Framework.Graphics;

namespace Nezuko.Interfaces
{
    public interface INZRenderable
    {
        void Draw(GraphicsDevice graphicsDevice, ICamera camera);
    }
}
