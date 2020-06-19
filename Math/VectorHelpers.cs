namespace Raytracer.Math
{
    public static class VectorHelpers
    {
        public static Vector3d GetReflectionVector(Vector3d direction, Vector3d normal)
        {
            return (direction - 2 * direction.Dot(normal) * normal).Normalize();
        }
    }
}