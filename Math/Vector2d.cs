using System;

namespace Raytracer.Math
{
    public class Vector2d
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2d()
        {
            X = 0.0f;
            Y = 0.0f;
        }

        public Vector2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2d(Vector2d a)
        {
            X = a.X;
            Y = a.Y;
        }

        public double Dot(Vector2d a)
        {
            return X * a.X + Y * a.Y;
        }

        public double Length()
        {
            return (double)System.Math.Sqrt(X * X + Y * Y);
        }

        public Vector2d Normalize()
        {
            double length = Length();

            X = X / length;
            Y = Y / length;

            return this;
        }

        public static Vector2d Normalize(double x, double y)
        {
            Vector2d result = new Vector2d(x, y);

            return result.Normalize();
        }

        public static Vector2d Normalize(Vector2d a)
        {
            Vector2d result = new Vector2d(a);

            return result.Normalize();
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }

        public static Vector2d operator -(Vector2d a, Vector2d b)
        {
            Vector2d result = new Vector2d();

            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;

            return result;
        }

        public static Vector2d operator +(Vector2d a, Vector2d b)
        {
            Vector2d result = new Vector2d();

            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;

            return result;
        }

        public static Vector2d operator *(double x, Vector2d a)
        {
            Vector2d result = new Vector2d();

            result.X = x * a.X;
            result.Y = x * a.Y;

            return result;
        }

        public static Vector2d operator *(Vector2d a, double x)
        {
            Vector2d result = new Vector2d();

            result.X = x * a.X;
            result.Y = x * a.Y;

            return result;
        }

        public static Vector2d operator /(Vector2d a, double x)
        {
            Vector2d result = new Vector2d();

            result.X = a.X / x;
            result.Y = a.Y / x;

            return result;
        }

        public static bool operator ==(Vector2d a, Vector2d b)
        {
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }
            else if (ReferenceEquals(b, null))
            {
                return false;
            }

            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2d a, Vector2d b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return (obj as Vector2d) == this;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;

                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();

                return hash;
            }
        }
    }
}