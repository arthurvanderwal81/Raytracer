using Raytracer.Math;
using Raytracer.Objects;

namespace Raytracer.Rendering.Intersection
{
    public class QuadIntersectionResult : IIntersectionResult
    {
        public IntersectionResult TriangleObjectIntersectionResult { get; set; }

        public double IntersectionDistance
        {
            get
            {
                return TriangleObjectIntersectionResult.IntersectionDistance;
            }
            set
            {
                TriangleObjectIntersectionResult.IntersectionDistance = value;
            }
        }

        public Vector3d Intersection
        {
            get
            {
                return TriangleObjectIntersectionResult.Intersection;
            }
            set
            {
                TriangleObjectIntersectionResult.Intersection = value;
            }
        }

        public AbstractObject3d Object { get; set; }

        public AbstractObject3d TriangleObject
        {
            get
            {
                return TriangleObjectIntersectionResult.Object;
            }
        }

        public QuadIntersectionResult(QuadObject quadObject, IntersectionResult triangleObjectIntersectionResult)
        {
            Object = quadObject;
            TriangleObjectIntersectionResult = triangleObjectIntersectionResult;
        }
        /*
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
        */
    }
}