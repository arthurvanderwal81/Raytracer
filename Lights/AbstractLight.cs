using Raytracer.Math;

namespace Raytracer.Lights
{
    public abstract class AbstractLight
    {
        public abstract Vector3d Position { get; }

        public Color3f DiffuseColor { get; protected set; }
        public double DiffusePower { get; protected set; }

        public Color3f SpecularColor { get; protected set; }
        public double SpecularPower { get; protected set; }

        public AbstractLight(Color3f diffuseColor)
        {
            DiffuseColor = diffuseColor;
            DiffusePower = 4f;

            SpecularColor = new Color3f(1f, 1f, 1f);
            SpecularPower = 3f;
        }

        public abstract Vector3d GetDirection(Vector3d position);

        public double GetDistance(Vector3d position)
        {
            if (Position == null)
            {
                return 1.0d;
            }

            return (position - Position).Length();
        }

        public double GetIntensity(Vector3d position, Vector3d surfaceNormal)
        {
            Vector3d lightDirection = GetDirection(position);

            double dot = -lightDirection.Dot(surfaceNormal);

            return dot < 0.0f ? 0.0f : dot;
        }
    }
}