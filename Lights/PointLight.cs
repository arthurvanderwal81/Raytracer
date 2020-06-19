using System.Drawing;

using Raytracer.Math;
using Raytracer.Rendering;

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

        public override Vector3d Intersection(Vector3d direction, Vector3d position)
        {
            return null;
            //return SphereHelpers.Intersection(_position, _radius, direction, position);
        }

        public override Color GetColor(Vector3d direction, Vector3d position, Scene scene)
        {
            Vector3d distanceVector = position - _position;
            double squaredDistance = distanceVector.Length();
            squaredDistance *= squaredDistance;

            //return Color.FromArgb((int)(255.0f - squaredDistance * 255.0f), (int)(255.0f - squaredDistance * 255.0f), (int)(255.0f - squaredDistance * 255.0f));
            return Color.White;
        }
    }
}