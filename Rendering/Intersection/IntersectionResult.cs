using Raytracer.Math;
using Raytracer.Objects;

namespace Raytracer.Rendering.Intersection
{
    public class IntersectionResult : IIntersectionResult
    {
        public double IntersectionDistance { get; set; }
        public Vector3d Intersection { get; set; }
        public AbstractObject3d Object { get; set; }

        public IntersectionResult()
        {
            IntersectionDistance = double.MaxValue;
        }
    }
}