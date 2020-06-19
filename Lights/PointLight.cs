using Raytracer.Math;

namespace Raytracer.Lights
{
    public class PointLight : AbstractLight
    {
        private Vector3d _position;
        //private double _radius;
        //private double _fallOff;

        public PointLight(Color3f diffuseColor, Vector3d position) : base(diffuseColor) //, double radius = 0.2f, double fallOff = 0.0f)
        {
            _position = position;
            //_radius = radius;
            //_fallOff = fallOff;
        }

        public override Vector3d Position
        {
            get
            {
                return _position;
            }
        }

        public override Vector3d GetDirection(Vector3d position)
        {
            return (position - _position).Normalize();
        }
    }
}