using Cirilla.Core.Models;
using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Rendering;

namespace Nezuko
{
    public static class NezukoLoader
    {
        public static Model LoadMOD3(GraphicsDevice graphicsDevice, string filePath)
        {
            var mod = new MOD3(filePath);
            var model = new Model();

            // Find MeshPart with lowest LOD (aka highest quality)
            int hqMeshIndex = 0;
            int lowestLod = int.MaxValue;

            for (int i = 0; i < mod.MeshParts.Count; i++)
                if (mod.MeshParts[i].Lod < lowestLod)
                {
                    hqMeshIndex = i;
                    lowestLod = mod.MeshParts[i].Lod;
                }

            model.Add(GetMesh(graphicsDevice, mod, hqMeshIndex));

            return model;
        }

        private static Mesh GetMesh(GraphicsDevice graphicsDevice, MOD3 mod, int meshPartIndex)
        {
            var vertexCount = mod.MeshParts[meshPartIndex].VertexCount;
            var indexCount = mod.MeshParts[meshPartIndex].IndexCount;
            var vertices = new VertexPositionNormalTexture[vertexCount];
            var indices = new short[indexCount];

            for (int i = 0; i < vertexCount; i++)
                vertices[i] = IVertexToVertexPositionNormalTexture(ref mod.VertexBuffers[meshPartIndex].Vertices[i]);

            int faceIndex = 0;
            for (int i = 0; i < indexCount; i += 3)
            {
                indices[i] = mod.Faces[meshPartIndex].Faces[faceIndex].Vertex3;
                indices[i + 1] = mod.Faces[meshPartIndex].Faces[faceIndex].Vertex2;
                indices[i + 2] = mod.Faces[meshPartIndex].Faces[faceIndex].Vertex1;
                faceIndex++;
            }

            var vertexLayout = new VertexDeclaration(VertexElement.Position<Vector3>(), VertexElement.Normal<Vector3>(), VertexElement.TextureCoordinate<Vector2>());
            var vertexBuffer = Buffer.Vertex.New(graphicsDevice, vertices);
            var vertexBufferBinding = new VertexBufferBinding(vertexBuffer, vertexLayout, vertexCount);
            var indexBuffer = Buffer.Index.New(graphicsDevice, indices);
            var indexBufferBinding = new IndexBufferBinding(indexBuffer, false, indexCount);

            var meshDraw = new MeshDraw
            {
                PrimitiveType = PrimitiveType.TriangleList,
                VertexBuffers = new[] { vertexBufferBinding },
                IndexBuffer = indexBufferBinding,
                DrawCount = indexCount
            };

            return new Mesh { Draw = meshDraw };
        }

        private static VertexPositionNormalTexture IVertexToVertexPositionNormalTexture(ref Cirilla.Core.Interfaces.IVertex vert)
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
