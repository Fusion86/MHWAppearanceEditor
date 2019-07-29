using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nezuko.Interfaces;

namespace Nezuko
{
    public class Camera : ICamera
    {
        // TODO: Make setters which call UpdateMatrices();
        public Vector3 Position { get; }
        public float FieldOfView { get; }
        public float NearPlane { get; }
        public float FarPlane { get; }
        public Vector3 Direction { get; }
        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }

        private GraphicsDevice graphicsDevice;

        public Camera(GraphicsDevice graphicsDevice, float fieldOfViewDegrees, float nearPlane, float farPlane, Vector3 position, Vector3 target)
        {
            this.graphicsDevice = graphicsDevice;
            FieldOfView = fieldOfViewDegrees * 0.0174532925f;
            Position = position;
            Direction = Vector3.Normalize(target - position);
            NearPlane = nearPlane;
            FarPlane = farPlane;
            UpdateMatrices();
        }

        public void Update(GameTime gameTime)
        {

        }

        private void UpdateMatrices()
        {
            ViewMatrix = Matrix.CreateLookAt(Position, Direction, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                FieldOfView,
                graphicsDevice.Viewport.AspectRatio,
                NearPlane,
                FarPlane);
        }
    }
}
