using Raytracer.Objects;

namespace Raytracer.Rendering.Intersection
{
    public class CubeIntersectionResult : QuadIntersectionResult
    {
        public QuadObject QuadObject { get; set; }

        public CubeIntersectionResult()
        {
        }

        public CubeIntersectionResult(QuadIntersectionResult intersectionResult, QuadObject quadObject, CubeObject cubeObject) : base(intersectionResult)
        {
            Object = cubeObject;
            QuadObject = quadObject;
        }
    }
}