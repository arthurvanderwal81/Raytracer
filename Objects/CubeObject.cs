using System.Collections.Generic;

using Raytracer.Math;
using Raytracer.Materials;

namespace Raytracer.Objects
{
    public class CubeObject : MeshObject
    {
        private List<Material> _materials;

        public CubeObject(string name, Vector3d position, double size, List<Material> materials) : base(name, materials[0])
        {
            _materials = materials;

            GenerateTriangles(position, size);
        }

        private void GenerateTriangles(Vector3d position, double size)
        {
            double e = 0.5f * size;

            Vector3d[] corners = new Vector3d[8]
            {
                new Vector3d(-e, -e, -e) + position,
                new Vector3d( e, -e, -e) + position,
                new Vector3d( e, -e,  e) + position,
                new Vector3d(-e, -e,  e) + position,

                new Vector3d(-e,  e, -e) + position,
                new Vector3d( e,  e, -e) + position,
                new Vector3d( e,  e,  e) + position,
                new Vector3d(-e,  e,  e) + position,
            };

            List<TriangleObject>  triangles = new List<TriangleObject>();
            
            // Bottom
            triangles.Add(new TriangleObject("CUBE_TRIANGLE_1",
                new Vertex(corners[2], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[1], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[0], new Vector2d(0.0f, 0.0f)),
                _materials[0]));

            triangles.Add(new TriangleObject("CUBE_TRIANGLE_2",
                new Vertex(corners[2], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[3], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[0], new Vector2d(0.0f, 0.0f)),
                _materials[1]));

            // Top
            triangles.Add(new TriangleObject("CUBE_TRIANGLE_3",
                new Vertex(corners[4], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[5], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[6], new Vector2d(0.0f, 0.0f)),
                _materials[2]));

            triangles.Add(new TriangleObject("CUBE_TRIANGLE_4",
                new Vertex(corners[6], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[7], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[0], new Vector2d(0.0f, 0.0f)),
                _materials[3]));

            // Left
            triangles.Add(new TriangleObject("CUBE_TRIANGLE_5",
                new Vertex(corners[7], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[4], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[0], new Vector2d(0.0f, 0.0f)),
                _materials[4]));

            triangles.Add(new TriangleObject("CUBE_TRIANGLE_6",
                new Vertex(corners[0], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[3], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[7], new Vector2d(0.0f, 0.0f)),
                _materials[5]));
            
            // Right
            triangles.Add(new TriangleObject("CUBE_TRIANGLE_7",
                new Vertex(corners[1], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[5], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[6], new Vector2d(0.0f, 0.0f)),
                _materials[6]));

            triangles.Add(new TriangleObject("CUBE_TRIANGLE_8",
                new Vertex(corners[6], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[2], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[1], new Vector2d(0.0f, 0.0f)),
                _materials[7]));

            // Front
            triangles.Add(new TriangleObject("CUBE_TRIANGLE_9",
                new Vertex(corners[0], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[1], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[5], new Vector2d(0.0f, 0.0f)),
                _materials[8]));

            triangles.Add(new TriangleObject("CUBE_TRIANGLE_10",
                new Vertex(corners[5], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[4], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[0], new Vector2d(0.0f, 0.0f)),
                _materials[9]));

            // Back
            triangles.Add(new TriangleObject("CUBE_TRIANGLE_11",
                new Vertex(corners[2], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[3], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[7], new Vector2d(0.0f, 0.0f)),
                _materials[10]));

            triangles.Add(new TriangleObject("CUBE_TRIANGLE_12",
                new Vertex(corners[7], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[6], new Vector2d(0.0f, 0.0f)),
                new Vertex(corners[2], new Vector2d(0.0f, 0.0f)),
                _materials[11]));

            UpdateTriangles(triangles);
        }
    }
}