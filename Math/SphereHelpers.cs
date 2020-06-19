namespace Raytracer.Math
{
    public static class SphereHelpers
    {
        public static Vector3d ProjectSpherePosition(Vector3d spherePosition, Vector3d direction, Vector3d position)
        {
            double directionFactor = (spherePosition - position).Dot(direction);

            if (directionFactor < 0.0f)
            {
                return null;
            }

            Vector3d spherePositionProjected = directionFactor * direction;

            return spherePositionProjected;
        }

        public static Vector3d Intersection(Vector3d spherePosition, double sphereRadius, Vector3d direction, Vector3d position)
        {
            Vector3d spherePositionProjected = ProjectSpherePosition(spherePosition, direction, position);

            if (spherePositionProjected == null)
            {
                return null;
            }

            Vector3d distance = (spherePosition - position) - spherePositionProjected;

            if (distance.Length() > sphereRadius)
            {
                return null;
            }

            double c = (double)System.Math.Sqrt(sphereRadius * sphereRadius - distance.Length() * distance.Length());

            double i1 = ((spherePosition - position).Dot(direction) - c);
            double i2 = ((spherePosition - position).Dot(direction) + c);

            return position + direction * i1;
        }
    }
}