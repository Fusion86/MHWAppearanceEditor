using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nezuko.Graphics
{
    public class NZModelMesh
    {
        public string Name { get; set; }
        public List<NZModelMeshPart> Parts { get; set; }
        public List<EffectMaterial> Materials { get; set; }
    }
}
