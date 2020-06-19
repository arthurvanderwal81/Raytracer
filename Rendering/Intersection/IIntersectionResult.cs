using Raytracer.Math;
using Raytracer.Objects;

namespace Raytracer.Rendering.Intersection
{
    public interface IIntersectionResult
    {
        double IntersectionDistance { get; set; }
        Vector3d Intersection { get; set; }
        AbstractObject3d Object { get; set; }
    }
}