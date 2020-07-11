using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Raytracer.Math;

namespace Raytracer.PostProcessing.Kernels
{
    public class Kernel
    {
        public int Size { get; set; }
        public int HalfSize { get; set; }

        private float[,] _values;

        public Kernel(int size)
        {
            Size = size;
            HalfSize = (Size - 1) / 2;

            _values = new float[size, size];
        }

        public float this[int y, int x]
        {
            get
            {
                return _values[y, x];
            }
            set
            {
                _values[y, x] = value;
            }
        }

        public float this[Vector2d p]
        {
            get
            {
                return _values[ScalarHelpers.Round(p.Y), ScalarHelpers.Round(p.X)];
            }
            set
            {
                _values[ScalarHelpers.Round(p.Y), ScalarHelpers.Round(p.X)] = value;
            }
        }

        public Vector2d GetCenter()
        {
            return new Vector2d((Size + 1) * 0.5, (Size + 1) * 0.5);
        }

        public bool ValidateSum(float sum)
        {
            float kernelSum = 0.0f;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    kernelSum += this[y, x];
                }
            }

            return kernelSum == sum;
        }

        private Rectangle Intersect(int x, int y, Rectangle rectangle)
        {
            Rectangle kernelRectangle = new Rectangle(x - HalfSize, y - HalfSize, Size, Size);

            return Rectangle.Intersect(kernelRectangle, rectangle);

            // intersect => max(lefts), max(tops), min(rights), min(bottoms)
        }

        public virtual KernelResult Apply(BitmapData bitmapData, int x, int y, Rectangle rectangle)
        {
            KernelResult result = new KernelResult();

            result.Rectangle = Intersect(x, y, rectangle);
            result.Value = 0.0f;

            for (int yy = result.Rectangle.Y; yy < result.Rectangle.Bottom; yy++)
            {
                for (int xx = result.Rectangle.X; xx < result.Rectangle.Left; xx++)
                {
                    // TODO: calc
                    result.Value += 0.0f;
                }
            }

            return result;
        }

        public static Kernel operator /(Kernel k, float f)
        {
            Kernel result = new Kernel(k.Size);

            for (int y = 0; y < k.Size; y++)
            {
                for (int x = 0; x < k.Size; x++)
                {
                    result[y, x] = k[y, x] / f;
                }
            }

            return result;
        }
    }
}
