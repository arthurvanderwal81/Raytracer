using Raytracer.Math;
using Raytracer.Materials;

namespace Raytracer.Objects
{
    public class PlaneObject : AbstractObject3d
    {
        private Vector3d _position;
        private Vector3d _normal;
        private Vector3d _uAxis;
        private Vector3d _vAxis;

        public PlaneObject(string name, Vector3d position, Vector3d normal, Material material)
        {
            _position = position;
            _normal = normal;

            Name = name;
            Material = material;

            GenerateUVAxes();
        }

        private void GenerateUVAxes()
        {
            Vector3d randomVector = new Vector3d(_normal.Y, _normal.X, _normal.Z);
            _vAxis = randomVector.Cross(_normal);
            _uAxis = _vAxis.Cross(_normal);

            _uAxis.Normalize();
            _vAxis.Normalize();                
        }

        public override Vector3d Intersection(Vector3d direction, Vector3d position)
        {
            double D = _position.Dot(_normal);
            double t = -(_normal.Dot(position) - D) / _normal.Dot(direction);

            if (t >= 0.0f)
            {
                return t * direction + position;
            }

            return null;
        }

        protected override Vector3d GetNormal(Vector3d positionOnObject)
        {
            return _normal;
        }

        protected override Vector2d GetUVCoordinates(Vector3d position)
        {
            Vector2d uvCoordinates = new Vector2d();

            uvCoordinates.X = _uAxis.Dot(position) % 1f;
            uvCoordinates.Y = _vAxis.Dot(position) % 1f;

            return uvCoordinates;
        }
    }
}