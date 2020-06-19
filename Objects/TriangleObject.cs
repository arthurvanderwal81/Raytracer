using Raytracer.Math;
using Raytracer.Materials;
using Raytracer.Rendering.Intersection;

namespace Raytracer.Objects
{
    public class TriangleObject : AbstractObject3d
    {
        private Vertex[] _vertices;
        private Vector3d[] _edges;
        private Vector3d _normal;

        public TriangleObject(string name, Vertex v0, Vertex v1, Vertex v2, Material material)
        {
            _vertices = new Vertex[] { v0, v1, v2 };
            _normal = CalculateNormal();

            Name = name;
            Material = material;

            GenerateEdges();
        }

        public TriangleObject(string name, Vertex v0, Vertex v1, Vertex v2, Vector3d normal, Material material)
        {
            _vertices = new Vertex[] { v0, v1, v2 };
            _normal = normal;

            Name = name;
            Material = material;

            GenerateEdges();
        }

        private Vector3d CalculateNormal()
        {
            Vector3d a = _vertices[1] - _vertices[0];
            Vector3d b = _vertices[2] - _vertices[0];

            return a.Cross(b).Normalize();
        }

        private void GenerateEdges()
        {
            _edges = new Vector3d[3];

            _edges[0] = _vertices[1] - _vertices[0];
            _edges[1] = _vertices[2] - _vertices[1];
            _edges[2] = _vertices[0] - _vertices[2];
        }

        public override Vector3d GetNormal(IIntersectionResult intersectionResult)
        {
            return _normal;
        }

        public override Vector2d GetUVCoordinates(IIntersectionResult intersectionResult)
        {
            //return new Vector2d(_edges[0].Dot(position), _edges[1].Dot(position));

            Vector3d f = intersectionResult.Intersection;
            Vector3d p1 = _vertices[0];
            Vector3d p2 = _vertices[1];
            Vector3d p3 = _vertices[2];

            var f1 = p1 - f;
            var f2 = p2 - f;
            var f3 = p3 - f;

            // calculate the areas and factors (order of parameters doesn't matter):
            double a = Vector3d.Cross(p1 - p2, p1 - p3).Length(); // main triangle area a
            double a1 = Vector3d.Cross(f2, f3).Length() / a; // p1's triangle area / a
            double a2 = Vector3d.Cross(f3, f1).Length() / a; // p2's triangle area / a 
            double a3 = Vector3d.Cross(f1, f2).Length() / a; // p3's triangle area / a
                                                            // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
            Vector2d uv = new Vector2d(_vertices[0].UV * a1 + _vertices[1].UV * a2 + _vertices[2].UV * a3);

            return uv;
        }

        public override IIntersectionResult Intersection(Vector3d direction, Vector3d position)
        {
            double D = _vertices[0].Dot(_normal);
            double t = -(_normal.Dot(position) - D) / _normal.Dot(direction);

            if (t >= 0.0f)
            {
                Vector3d planeIntersecion = t * direction + position;
                Vector3d P = planeIntersecion;
                Vector3d v0 = _vertices[0];
                Vector3d v1 = _vertices[1];
                Vector3d v2 = _vertices[2];

                Vector3d edge0 = _edges[0];
                Vector3d edge1 = _edges[1];
                Vector3d edge2 = _edges[2];

                Vector3d C0 = P - v0;
                Vector3d C1 = P - v1;
                Vector3d C2 = P - v2;

                if (_normal.Dot(edge0.Cross(C0)) > 0f &&
                    _normal.Dot(edge1.Cross(C1)) > 0f &&
                    _normal.Dot(edge2.Cross(C2)) > 0f)
                {
                    IntersectionResult intersectionResult = new IntersectionResult();

                    intersectionResult.Intersection = planeIntersecion;
                    intersectionResult.IntersectionDistance = (intersectionResult.Intersection - position).Length();
                    intersectionResult.Object = this;

                    return intersectionResult;
                }
            }

            return null;
        }
    }
}