using Raytracer.Math;
using Raytracer.Rendering;
using Raytracer.Materials;
using Raytracer.Rendering.Intersection;

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

        public override Vector3d GetNormal(IIntersectionResult intersectionResult)
        {
            return _normal;
        }

        public override Vector2d GetUVCoordinates(IIntersectionResult intersectionResult)
        {
            Vector3d position = intersectionResult.Intersection;
            Vector2d uvCoordinates = new Vector2d();

            uvCoordinates.X = _uAxis.Dot(position) % 1f;
            uvCoordinates.Y = _vAxis.Dot(position) % 1f;

            return uvCoordinates;
        }

        public override bool IsVisible(Camera camera)
        {
            return true;
        }

        public override IIntersectionResult Intersection(Vector3d direction, Vector3d position)
        {
            double D = _position.Dot(_normal);
            double t = -(_normal.Dot(position) - D) / _normal.Dot(direction);

            if (t >= 0.0f)
            {
                IntersectionResult intersectionResult = new IntersectionResult();

                intersectionResult.Intersection = t * direction + position;
                intersectionResult.IntersectionDistance = (intersectionResult.Intersection - position).Length();
                intersectionResult.Object = this;

                return intersectionResult;
            }

            return null;
        }
    }
}