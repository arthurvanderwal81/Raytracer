using System.Drawing;
using System.Collections.Generic;

using Raytracer.Math;
using Raytracer.Materials;
using Raytracer.Rendering;
using Raytracer.Rendering.Intersection;

namespace Raytracer.Objects
{
    public class MeshObject : AbstractObject3d
    {
        private List<TriangleObject> _triangles;

        public MeshObject(string name, Material material)
        {
            _triangles = new List<TriangleObject>();

            Name = name;
            Material = material;
        }

        protected void UpdateTriangles(List<TriangleObject> triangles)
        {
            _triangles = triangles;
        }

        public override Vector3d GetNormal(IIntersectionResult intersectionResult)
        {
            return (intersectionResult as MeshIntersectionResult).TriangleObject.GetNormal(intersectionResult);
        }

        public override Vector2d GetUVCoordinates(IIntersectionResult intersectionResult)
        {
            return (intersectionResult as MeshIntersectionResult).TriangleObject.GetUVCoordinates(intersectionResult);
        }

        public override bool IsVisible(Camera camera)
        {
            foreach (TriangleObject triangle in _triangles)
            {
                if (triangle.IsVisible(camera))
                {
                    return true;
                }
            }

            return false;
        }

        public override IIntersectionResult Intersection(Vector3d direction, Vector3d position)
        {
            foreach (TriangleObject triangle in _triangles)
            {
                IIntersectionResult intersectionResult = triangle.Intersection(direction, position);

                if (intersectionResult != null)
                {
                    return new MeshIntersectionResult(intersectionResult, this);
                }
            }

            return null;
        }

        public override Color GetColor(Vector3d direction, IIntersectionResult intersectionResult, Scene scene)
        {
            return (intersectionResult as MeshIntersectionResult).TriangleObject.GetColor(direction, intersectionResult, scene);
        }
    }
}