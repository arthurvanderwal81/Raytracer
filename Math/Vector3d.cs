using System;

namespace Raytracer.Math
{
    public class Vector3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3d()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3d(Vector3d a)
        {
            X = a.X;
            Y = a.Y;
            Z = a.Z;
        }

        public double Dot(Vector3d a)
        {
            return X * a.X + Y * a.Y + Z * a.Z;
        }

        public Vector3d Cross(Vector3d b)
        {
            return Vector3d.Cross(this, b);
            /*
            Vector3d result = new Vector3d();

            result.X = Y * b.Z - Z * b.Y;
            result.Y = Z * b.X - X * b.Z;
            result.Z = X * b.Y - Y * b.X;

            return result;
            */
        }

        public static Vector3d Cross(Vector3d a, Vector3d b)
        {
            Vector3d result = new Vector3d();

            result.X = a.Y * b.Z - a.Z * b.Y;
            result.Y = a.Z * b.X - a.X * b.Z;
            result.Z = a.X * b.Y - a.Y * b.X;

            return result;
        }

        public double Length()
        {
            return System.Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3d Normalize()
        {
            double length = Length();

            X = X / length;
            Y = Y / length;
            Z = Z / length;

            return this;
        }

        public static Vector3d Normalize(double x, double y, double z)
        {
            Vector3d result = new Vector3d(x, y, z);

            return result.Normalize();
        }

        public static Vector3d Normalize(Vector3d a)
        {
            Vector3d result = new Vector3d(a);

            return result.Normalize();
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }

        public static Vector3d operator -(Vector3d a)
        {
            Vector3d result = new Vector3d();

            result.X = -a.X;
            result.Y = -a.Y;
            result.Z = -a.Z;

            return result;
        }

        public static Vector3d operator -(Vector3d a, Vector3d b)
        {
            Vector3d result = new Vector3d();

            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;

            return result;
        }

        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            Vector3d result = new Vector3d();

            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;

            return result;
        }

        public static Vector3d operator *(double x, Vector3d a)
        {
            Vector3d result = new Vector3d();

            result.X = x * a.X;
            result.Y = x * a.Y;
            result.Z = x * a.Z;

            return result;
        }

        public static Vector3d operator *(Vector3d a, double x)
        {
            Vector3d result = new Vector3d();

            result.X = x * a.X;
            result.Y = x * a.Y;
            result.Z = x * a.Z;

            return result;
        }

        public static bool operator ==(Vector3d a, Vector3d b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }
            else if (object.ReferenceEquals(b, null))
            {
                return false;
            }

            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vector3d a, Vector3d b)
        {
            return !(a == b);
        }
    }
}