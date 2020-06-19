using System.Drawing;
using System.Collections.Generic;

using Raytracer.Math;
using Raytracer.Materials;
using Raytracer.Rendering;

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

        protected override Vector3d GetNormal(Vector3d positionOnObject)
        {
            throw new System.NotImplementedException();
        }

        protected override Vector2d GetUVCoordinates(Vector3d positionOnObject)
        {
            throw new System.NotImplementedException();
        }

        public override Vector3d Intersection(Vector3d direction, Vector3d position)
        {
            foreach (TriangleObject triangle in _triangles)
            {
                Vector3d intersection = triangle.Intersection(direction, position);

                if (intersection != null)
                {
                    return intersection;
                }
            }

            return null;
        }

        public override Color GetColor(Vector3d direction, Vector3d position, Scene scene)
        {
            foreach (TriangleObject triangle in _triangles)
            {
                Vector3d intersection = triangle.Intersection(direction, position);

                if (intersection != null)
                {
                    return triangle.GetColor(direction, position, scene);
                }
            }

            return Color.Black;
        }
    }
}