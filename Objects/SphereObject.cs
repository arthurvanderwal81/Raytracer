using Raytracer.Math;
using Raytracer.Rendering;
using Raytracer.Materials;
using Raytracer.Rendering.Intersection;

namespace Raytracer.Objects
{
    public class SphereObject : AbstractObject3d
    {
        private Vector3d _position;
        private double _radius;

        public SphereObject(string name, Vector3d position, double radius, Material material)
        {
            _position = position;
            _radius = radius;

            Name = name;
            Material = material;
        }

        private Vector3d ProjectSpherePosition(Vector3d direction, Vector3d position)
        {
            double directionFactor = (_position - position).Dot(direction);

            if (directionFactor < 0.0f)
            {
                return null;
            }

            Vector3d spherePositionProjected = directionFactor * direction;

            return spherePositionProjected;
        }

        public override Vector3d GetNormal(IIntersectionResult intersectionResult)
        {
            Vector3d normal = intersectionResult.Intersection - _position;

            if (Material.InvertNormals)
            {
                normal = -normal;
            }

            return normal.Normalize();
        }

        public override bool IsVisible(Camera camera)
        {
            return (camera.Direction.Dot(_position - camera.Position) + _radius) >= 0d;
        }

        public override IIntersectionResult Intersection(Vector3d direction, Vector3d position)
        {
            Vector3d spherePositionProjected = ProjectSpherePosition(direction, position);

            if (spherePositionProjected == null)
            {
                return null;
            }

            Vector3d distance = (_position - position) - spherePositionProjected;

            if (distance.Length() > _radius)
            {
                return null;
            }

            double c = System.Math.Sqrt(_radius * _radius - distance.Length() * distance.Length());

            double i1 = ((_position - position).Dot(direction) - c);
            double i2 = ((_position - position).Dot(direction) + c);

            IntersectionResult intersectionResult = new IntersectionResult();

            intersectionResult.Intersection = position + direction * i1;
            intersectionResult.IntersectionDistance = (intersectionResult.Intersection - position).Length();
            intersectionResult.Object = this;

            return intersectionResult;
        }

        public override Vector2d GetUVCoordinates(IIntersectionResult intersectionResult)
        {
            Vector3d normal = GetNormal(intersectionResult);

            double u = (System.Math.Atan2(normal.X, normal.Z) / (2.0f * System.Math.PI) + 0.5f);
            double v = -normal.Y * 0.5f + 0.5f;

            return new Vector2d(u, v);
        }
    }
}