using Raytracer.Math;
using Raytracer.Materials;

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

        protected override Vector3d GetNormal(Vector3d positionOnSphere)
        {
            Vector3d normal = positionOnSphere - _position;

            if (Material.InvertNormals)
            {
                normal = -normal;
            }

            return normal.Normalize();
        }

        public override Vector3d Intersection(Vector3d direction, Vector3d position)
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

            return position + direction * i1;
        }

        protected override Vector2d GetUVCoordinates(Vector3d position)
        {
            Vector3d normal = GetNormal(position);

            double u = (double)(System.Math.Atan2(normal.X, normal.Z) / (2.0f * System.Math.PI) + 0.5f);
            double v = -normal.Y * 0.5f + 0.5f;

            return new Vector2d(u, v);
        }
    }
}