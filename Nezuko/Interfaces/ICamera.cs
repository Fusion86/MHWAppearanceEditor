using Microsoft.Xna.Framework;

namespace Nezuko.Interfaces
{
    public interface ICamera : INZUpdateable
    {
        Matrix ViewMatrix { get; }
        Matrix ProjectionMatrix { get; }
    }
}
