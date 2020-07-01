using System;
using System.Drawing;

namespace Raytracer.Math
{
    public class Color3f : Vector3d
    {
        public double R
        {
            get
            {
                return X;
            }
        }

        public double G
        {
            get
            {
                return Y;
            }
        }

        public double B
        {
            get
            {
                return Z;
            }
        }

        public Color3f() : base()
        {
        }

        public Color3f(double r, double g, double b) : base(r, g, b)
        {
        }

        public Color3f(Color color) : base(color.R / 255.0, color.G / 255.0, color.B / 255.0)
        {
        }

        unsafe public Color3f(byte* bitmap) : base(bitmap[2] / 255.0, bitmap[1] / 255.0, bitmap[0] / 255.0)
        {
        }

        public static Color3f operator -(Color3f a, Vector3d b)
        {
            Color3f result = new Color3f();

            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;

            return result;
        }

        public static Color3f operator +(Color3f a, Vector3d b)
        {
            Color3f result = new Color3f();

            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;

            return result;
        }

        public static Color3f operator *(double x, Color3f a)
        {
            Color3f result = new Color3f();

            result.X = x * a.X;
            result.Y = x * a.Y;
            result.Z = x * a.Z;

            return result;
        }

        public static Color3f operator *(Color3f a, double x)
        {
            Color3f result = new Color3f();

            result.X = x * a.X;
            result.Y = x * a.Y;
            result.Z = x * a.Z;

            return result;
        }

        public static Color3f operator *(Color3f a, Color3f b)
        {
            Color3f result = new Color3f();

            result.X = a.X * b.X;
            result.Y = a.Y * b.Y;
            result.Z = a.Z * b.Z;

            return result;
        }

        public static Color3f operator /(Color3f a, double x)
        {
            Color3f result = new Color3f();

            result.X = a.X / x;
            result.Y = a.Y / x;
            result.Z = a.Z / x;

            return result;
        }

        public static bool operator ==(Color3f a, Color3f b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public static bool operator !=(Color3f a, Color3f b)
        {
            return !(a == b);
        }

        public Color ToColor()
        {
            return Color.FromArgb((int)(ScalarHelpers.Saturate(R) * 255f), (int)(ScalarHelpers.Saturate(G) * 255f), (int)(ScalarHelpers.Saturate(B) * 255f));
        }
    }
}