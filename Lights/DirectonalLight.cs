using Raytracer.Math;
using Raytracer.Lights;

namespace Raytracer
{
    public class DirectonalLight : AbstractLight
    {
        private Vector3d _direction;

        public DirectonalLight(Color3f diffuseColor, Vector3d direction) : base(diffuseColor)
        {
            _direction = direction;
            DiffusePower = 1d;
        }

        public override Vector3d Position
        {
            get
            {
                return null;
            }
        }

        public override Vector3d GetDirection(Vector3d position)
        {
            return _direction;
        }
    }
}