using Raytracer.Objects;

namespace Raytracer.Rendering.Intersection
{
    public class QuadIntersectionResult : IntersectionResult
    {
        public TriangleObject TriangleObject { get; set; }

        public QuadIntersectionResult()
        {
        }

        public QuadIntersectionResult(QuadIntersectionResult quadIntersection)
        {
            IntersectionDistance = quadIntersection.IntersectionDistance;
            Intersection = quadIntersection.Intersection;
            Object = quadIntersection.Object;
            TriangleObject = quadIntersection.TriangleObject;
        }

        public QuadIntersectionResult(IIntersectionResult intersectionResult, QuadObject quadObject)
        {
            IntersectionDistance = intersectionResult.IntersectionDistance;
            Intersection = intersectionResult.Intersection;
            Object = quadObject;
            TriangleObject = intersectionResult.Object as TriangleObject;
        }
    }
}