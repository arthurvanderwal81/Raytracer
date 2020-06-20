using Raytracer.Math;
using Raytracer.Objects;

namespace Raytracer.Rendering.Intersection
{
    public class CubeIntersectionResult : IIntersectionResult
    {
        public QuadIntersectionResult QuadObjectIntersectionResult { get; set; }

        public double IntersectionDistance
        {
            get
            {
                return QuadObjectIntersectionResult.IntersectionDistance;
            }
            set
            {
                QuadObjectIntersectionResult.IntersectionDistance = value;
            }
        }

        public Vector3d Intersection
        {
            get
            {
                return QuadObjectIntersectionResult.Intersection;
            }
            set
            {
                QuadObjectIntersectionResult.Intersection = value;
            }
        }

        public AbstractObject3d Object { get; set; }

        public AbstractObject3d TriangleObject
        {
            get
            {
                return QuadObjectIntersectionResult.TriangleObject;
            }
        }

        public CubeIntersectionResult(CubeObject cubeObject, QuadIntersectionResult quadObjectIntersectionResult)
        {
            Object = cubeObject;
            QuadObjectIntersectionResult = quadObjectIntersectionResult;
        }
    }
}