using Microsoft.Xna.Framework;

namespace Nezuko.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Called INezukoUpdateable because MonoGame already has a IUpdateable</remarks>
    public interface INZUpdateable
    {
        void Update(GameTime gameTime);
    }
}
