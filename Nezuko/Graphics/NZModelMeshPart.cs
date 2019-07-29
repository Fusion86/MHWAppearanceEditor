using Microsoft.Xna.Framework.Graphics;

namespace Nezuko.Graphics
{
    public class NZModelMeshPart
    {
        public int VertexCount { get; set; }
        public int IndexCount { get; set; }

        public VertexBuffer VertexBuffer { get; set; }
        public VertexDeclaration VertexDeclaration { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public EffectMaterial Material { get; set; }
    }
}
