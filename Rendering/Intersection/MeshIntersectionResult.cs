using Raytracer.Objects;

namespace Raytracer.Rendering.Intersection
{
    public class MeshIntersectionResult : IntersectionResult
    {
        public TriangleObject TriangleObject { get; set; }

        public MeshIntersectionResult()
        {
        }

        public MeshIntersectionResult(IIntersectionResult intersectionResult, MeshObject meshObject)
        {
            IntersectionDistance = intersectionResult.IntersectionDistance;
            Intersection = intersectionResult.Intersection;
            Object = meshObject;
            TriangleObject = intersectionResult.Object as TriangleObject;
        }
    }
}