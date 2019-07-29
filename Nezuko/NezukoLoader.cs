using MOD3 = Cirilla.Core.Models.MOD3;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nezuko.Graphics;

namespace Nezuko
{
    public class NezukoLoader
    {
        public ModelData LoadMod3(GraphicsDevice graphicsDevice, string filePath)
        {
            var mod = new MOD3(filePath);

            ModelData modelData = new ModelData();
            modelData.Meshes.Add(GetMesh(graphicsDevice, mod));

            return modelData;
        }

        public NZModelMesh GetMesh(GraphicsDevice graphicsDevice, MOD3 mod)
        {
            NZModelMesh mesh = new NZModelMesh();
            mesh.Name = "todo";
            mesh.Parts = new List<NZModelMeshPart>();

            for (int meshIndex = 0; meshIndex < mod.Header.MeshCount; meshIndex++)
            {
                var modelMeshPart = new NZModelMeshPart();
                modelMeshPart.VertexCount = mod.MeshParts[meshIndex].VertexCount;
                modelMeshPart.IndexCount = mod.MeshParts[meshIndex].IndexCount;
                modelMeshPart.VertexDeclaration = VertexPositionNormalTexture.VertexDeclaration;

                // Vertex buffer
                var vertexData = new VertexPositionNormalTexture[modelMeshPart.VertexCount];
                for (int j = 0; j < modelMeshPart.VertexCount; j++)
                    vertexData[j] = GetVertexPositionNormalTexture(ref mod.VertexBuffers[meshIndex].Vertices[j]);

                var vertexBuffer = new VertexBuffer(graphicsDevice, modelMeshPart.VertexDeclaration, modelMeshPart.VertexCount, BufferUsage.WriteOnly);
                vertexBuffer.SetData(vertexData);
                modelMeshPart.VertexBuffer = vertexBuffer;

                // Index buffer
                var indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, modelMeshPart.IndexCount, BufferUsage.WriteOnly);
                var indexData = new short[modelMeshPart.IndexCount];

                int faceIndex = 0;
                for (int j = 0; j < modelMeshPart.IndexCount; j += 3)
                {
                    indexData[j] = mod.Faces[meshIndex].Faces[faceIndex].Vertex3;
                    indexData[j + 1] = mod.Faces[meshIndex].Faces[faceIndex].Vertex2;
                    indexData[j + 2] = mod.Faces[meshIndex].Faces[faceIndex].Vertex1;
                    faceIndex++;
                }

                indexBuffer.SetData(indexData);
                modelMeshPart.IndexBuffer = indexBuffer;
                mesh.Parts.Add(modelMeshPart);
            }

            return mesh;
        }

        public VertexPositionNormalTexture GetVertexPositionNormalTexture(ref Cirilla.Core.Interfaces.IVertex vert)
        {
            return new VertexPositionNormalTexture
            {
                Position = new Vector3(vert.Position.X, vert.Position.Y, vert.Position.Z),
                Normal = new Vector3(vert.Normal.X, vert.Normal.Y, vert.Normal.Z),
                TextureCoordinate = new Vector2(vert.Uv.X, vert.Uv.Y)
            };
        }
    }
}
