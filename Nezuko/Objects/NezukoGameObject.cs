using Microsoft.Xna.Framework;

namespace Nezuko.Objects
{
    public class NezukoGameObject
    {
        public Vector3 Location { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Velocity { get; set; }
        public float Scale { get; set; } = 1;
}
}
