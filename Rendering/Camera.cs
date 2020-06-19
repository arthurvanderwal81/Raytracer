using Raytracer.Math;

namespace Raytracer.Rendering
{
    public class Camera
    {
        private int _screenResolutionX;
        private int _screenResolutionY;
        private double _aspectRatio;
        private double _projectionPlaneOffset;

        private double _fov = 90.0f;

        public Vector3d Position { get; set; }
        public Vector3d Direction { get; set; }
        public double FocalLength { get; set; }

        public Camera(int screenResolutionX, int screenResolutionY, double focalLength)
        {
            _screenResolutionX = screenResolutionX;
            _screenResolutionY = screenResolutionY;
            _aspectRatio = (double)_screenResolutionX / (double)_screenResolutionY;
            _projectionPlaneOffset = 0.5f * _aspectRatio / (double)System.Math.Tan(0.5 * (_fov / 180.0f) * System.Math.PI);

            Position = new Vector3d(0f, 1f, 0f);
            Direction = new Vector3d(1.0f, 0.0f, 0.0f);
            FocalLength = focalLength;
        }

        public void LookAt(Vector3d position)
        {
            Direction = (position - Position).Normalize();
        }

        public void FocusOn(Vector3d focusPoint)
        {
            FocalLength = (focusPoint - Position).Length();
        }

        public Vector3d ScreenSpaceToWorldSpace(int screenX, int screenY)
        {
            // Transform screenspace to normalized screenspace (-1, -1) -> (1, 1)            

            Vector3d normalizedScreenSpace = new Vector3d((double)screenX / (double)_screenResolutionX * _aspectRatio - _aspectRatio * .5f, (double)screenY / (double)_screenResolutionY - .5f, 0.0f);

            Vector3d worldSpaceZ = Direction;
            Vector3d worldSpaceY = new Vector3d(0f, 1f, 0f);
            Vector3d worldSpaceX = worldSpaceZ.Cross(worldSpaceY);
            worldSpaceY = worldSpaceX.Cross(worldSpaceZ);

            worldSpaceY.Normalize();
            worldSpaceX.Normalize();

            Vector3d worldSpace = Position + Direction * _projectionPlaneOffset + worldSpaceX * normalizedScreenSpace.X + worldSpaceY * -normalizedScreenSpace.Y;

            return worldSpace;
        }

        public Vector3d GetFocalPoint(int screenX, int screenY)
        {
            return GetFocalPoint(ScreenSpaceToWorldSpace(screenX, screenY));
        }

        public Vector3d GetFocalPoint(Vector3d pixelWorldSpace)
        {
            Vector3d rayDirection = Vector3d.Normalize(pixelWorldSpace - Position);

            double d = _projectionPlaneOffset;
            double dm = (pixelWorldSpace - Position).Length();

            Vector3d focalPoint = dm / (d / (d + FocalLength)) * rayDirection + Position;

            return focalPoint;
        }

        public Vector3d GetRayDirection(int screenX, int screenY)
        {
            Vector3d worldSpace = ScreenSpaceToWorldSpace(screenX, screenY);

            return Vector3d.Normalize(worldSpace - Position);
        }
    }
}