using System.Drawing;

using Raytracer.Math;
using Raytracer.Rendering;
using Raytracer.Materials;
using Raytracer.Rendering.Intersection;

namespace Raytracer.Objects
{
    public class CubeObject : AbstractObject3d
    {
        private QuadObject[] _quads;

        public CubeObject(string name, Vector3d position, double size, Material material)
        {
            Name = name;
            Material = material;

            GenerateQuads(position, size);
        }

        private void GenerateQuads(Vector3d position, double size)
        {
            double e = 0.5f * size;

            Vector3d[] corners = new Vector3d[8]
            {
                new Vector3d(-e, -e, -e) + position, //0
                new Vector3d( e, -e, -e) + position, //1
                new Vector3d( e, -e,  e) + position, //2
                new Vector3d(-e, -e,  e) + position, //3

                new Vector3d(-e,  e, -e) + position, //4
                new Vector3d( e,  e, -e) + position, //5
                new Vector3d( e,  e,  e) + position, //6
                new Vector3d(-e,  e,  e) + position, //7
            };

            _quads = new QuadObject[6];

            // Front
            _quads[0] = new QuadObject(string.Format("{0}_QUAD_1", Name), 
                new Vertex(corners[0], new Vector2d(0.0, 0.0)),
                new Vertex(corners[3], new Vector2d(0.0, 1.0)),
                new Vertex(corners[7], new Vector2d(1.0, 1.0)),
                new Vertex(corners[4], new Vector2d(1.0, 0.0)),
                Material
            );

            _quads[1] = new QuadObject(string.Format("{0}_QUAD_2", Name),
                new Vertex(corners[2], new Vector2d(0.0, 0.0)),
                new Vertex(corners[1], new Vector2d(0.0, 1.0)),
                new Vertex(corners[5], new Vector2d(1.0, 1.0)),
                new Vertex(corners[6], new Vector2d(1.0, 0.0)),
                Material
            );

            _quads[2] = new QuadObject(string.Format("{0}_QUAD_3", Name),
                new Vertex(corners[3], new Vector2d(0.0, 0.0)),
                new Vertex(corners[2], new Vector2d(0.0, 1.0)),
                new Vertex(corners[6], new Vector2d(1.0, 1.0)),
                new Vertex(corners[7], new Vector2d(1.0, 0.0)),
                Material
            );

            _quads[3] = new QuadObject(string.Format("{0}_QUAD_4", Name),
                new Vertex(corners[1], new Vector2d(0.0, 0.0)),
                new Vertex(corners[0], new Vector2d(0.0, 1.0)),
                new Vertex(corners[4], new Vector2d(1.0, 1.0)),
                new Vertex(corners[5], new Vector2d(1.0, 0.0)),
                Material
            );

            _quads[4] = new QuadObject(string.Format("{0}_QUAD_5", Name),
                new Vertex(corners[4], new Vector2d(0.0, 0.0)),
                new Vertex(corners[7], new Vector2d(0.0, 1.0)),
                new Vertex(corners[6], new Vector2d(1.0, 1.0)),
                new Vertex(corners[5], new Vector2d(1.0, 0.0)),
                Material
            );

            _quads[5] = new QuadObject(string.Format("{0}_QUAD_6", Name),
                new Vertex(corners[0], new Vector2d(0.0, 0.0)),
                new Vertex(corners[1], new Vector2d(0.0, 1.0)),
                new Vertex(corners[2], new Vector2d(1.0, 1.0)),
                new Vertex(corners[3], new Vector2d(0.0, 0.0)),
                Material
            );

            _quads[0].Parent = this;
            _quads[1].Parent = this;
            _quads[2].Parent = this;
            _quads[3].Parent = this;
            _quads[4].Parent = this;
            _quads[5].Parent = this;

            /*
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
            */
        }

        public override Vector3d GetNormal(IIntersectionResult intersectionResult)
        {
            CubeIntersectionResult cubeIntersectionResult = intersectionResult as CubeIntersectionResult;

            return cubeIntersectionResult.TriangleObject.GetNormal(cubeIntersectionResult.QuadObjectIntersectionResult.TriangleObjectIntersectionResult);
        }

        public override Vector2d GetUVCoordinates(IIntersectionResult intersectionResult)
        {
            CubeIntersectionResult cubeIntersectionResult = intersectionResult as CubeIntersectionResult;

            return cubeIntersectionResult.TriangleObject.GetUVCoordinates(cubeIntersectionResult.QuadObjectIntersectionResult.TriangleObjectIntersectionResult);
        }

        public override bool IsVisible(Camera camera)
        {
            foreach (QuadObject quadObject in _quads)
            {
                if (quadObject.IsVisible(camera))
                {
                    return true;
                }
            }

            return false;
        }

        public override IIntersectionResult Intersection(Vector3d direction, Vector3d position)
        {
            IIntersectionResult nearestIntersectionResult = null;

            foreach (QuadObject quadObject in _quads)
            {
                //QuadObject quadObject = _quads[3];
                IIntersectionResult intersectionResult = quadObject.Intersection(direction, position);

                if (intersectionResult != null)
                {
                    if (nearestIntersectionResult == null)
                    {
                        nearestIntersectionResult = intersectionResult;
                    }
                    else
                    {
                        if (intersectionResult.IntersectionDistance < nearestIntersectionResult.IntersectionDistance)
                        {
                            nearestIntersectionResult = intersectionResult;
                        }
                    }
                }
            }

            if (nearestIntersectionResult != null)
            {
                return new CubeIntersectionResult(this, nearestIntersectionResult as QuadIntersectionResult);
            }

            return null;
        }

        public override Color GetColor(Vector3d direction, IIntersectionResult intersectionResult, Scene scene)
        {
            CubeIntersectionResult cubeIntersectionResult = intersectionResult as CubeIntersectionResult;

            return cubeIntersectionResult.TriangleObject.GetColor(direction, cubeIntersectionResult.QuadObjectIntersectionResult.TriangleObjectIntersectionResult, scene);
        }
    }
}