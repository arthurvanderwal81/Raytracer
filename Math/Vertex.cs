namespace Raytracer.Math
{
    public class Vertex : Vector3d
    {
        public Vector2d UV { get; set; }

        public Vertex()
        {
        }

        public Vertex(Vector3d v) : base(v)
        {
        }

        public Vertex(Vector3d v, Vector2d t) : base(v)
        {
            UV = t;
        }

        public Vertex(double x, double y, double z) : base(x, y, z)
        {
        }

        public Vertex(double x, double y, double z, double u, double v) : base(x, y, z)
        {
            UV = new Vector2d(u, v);
        }
    }
}