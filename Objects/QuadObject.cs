using System;
using System.Drawing;

using Raytracer.Math;
using Raytracer.Materials;
using Raytracer.Rendering;
using Raytracer.Rendering.Intersection;

namespace Raytracer.Objects
{
    public class QuadObject : AbstractObject3d
    {
        private TriangleObject[] _triangles;

        public QuadObject(string name, Vertex v0, Vertex v1, Vertex v2, Vertex v3, Material material)
        {
            Name = name;
            Material = material;

            GenerateTriangles(v0, v1, v2, v3);
        }

        private void GenerateTriangles(Vertex v0, Vertex v1, Vertex v2, Vertex v3)
        {
            _triangles = new TriangleObject[2];

            _triangles[0] = new TriangleObject(string.Format("{0}_TRIANGLE_1", Name), v0, v1, v2, Material);
            _triangles[1] = new TriangleObject(string.Format("{0}_TRIANGLE_2", Name), v2, v3, v0, Material);

            _triangles[0].Parent = this;
            _triangles[1].Parent = this;

            if (_triangles[0].GetNormal(null) != _triangles[1].GetNormal(null))
            {
                throw new Exception("Triangles are not in the same plane");
            }
        }

        public override Vector3d GetNormal(IIntersectionResult intersectionResult)
        {
            QuadIntersectionResult quadIntersectionResult = intersectionResult as QuadIntersectionResult;

            return quadIntersectionResult.TriangleObjectIntersectionResult.Object.GetNormal(quadIntersectionResult.TriangleObjectIntersectionResult);
        }

        public override Vector2d GetUVCoordinates(IIntersectionResult intersectionResult)
        {
            QuadIntersectionResult quadIntersectionResult = intersectionResult as QuadIntersectionResult;

            return quadIntersectionResult.TriangleObjectIntersectionResult.Object.GetUVCoordinates(quadIntersectionResult.TriangleObjectIntersectionResult);
        }

        public override bool IsVisible(Camera camera)
        {
            return _triangles[0].IsVisible(camera) || _triangles[1].IsVisible(camera);
        }

        public override IIntersectionResult Intersection(Vector3d direction, Vector3d position)
        {
            IIntersectionResult result;

            result = _triangles[0].Intersection(direction, position);

            if (result != null)
            {
                return new QuadIntersectionResult(this, result as IntersectionResult);
            }

            result = _triangles[1].Intersection(direction, position);

            if (result != null)
            {
                return new QuadIntersectionResult(this, result as IntersectionResult);
            }

            return null;
        }

        public override Color GetColor(Vector3d direction, IIntersectionResult intersectionResult, Scene scene)
        {
            QuadIntersectionResult quadIntersectionResult = intersectionResult as QuadIntersectionResult;

            return quadIntersectionResult.TriangleObjectIntersectionResult.Object.GetColor(direction, quadIntersectionResult.TriangleObjectIntersectionResult, scene);
        }
    }
}