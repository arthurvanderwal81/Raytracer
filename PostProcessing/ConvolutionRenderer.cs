using Raytracer.Math;
using System.Drawing;

namespace Raytracer.PostProcessing
{
    //http://chemaguerra.com/circular-radial-blur/
    //http://www.computer-darkroom.com/ps_tutorials/tutorial_9_1.htm
    //https://gaming.stackexchange.com/questions/306721/what-is-radial-blur
    //https://www.photoshopessentials.com/photo-effects/radial-blur-action-effect-photoshop/
    //https://www.imgonline.com.ua/eng/blur-radial.php
    public class ConvolutionRenderer
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

        protected Bitmap _bitmap;
        protected Kernel _kernel;
        
        public ConvolutionRenderer(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public ConvolutionRenderer(Bitmap bitmap, Kernel kernel)
        {
            _bitmap = bitmap;
            _kernel = kernel;
        }

        public unsafe static Color3f ApplyKernel(System.Drawing.Imaging.BitmapData bitmapData, Kernel kernel, int x, int y)
        {
            /*
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            */
            byte* scan0 = (byte*)bitmapData.Scan0.ToPointer();

            int halfKernelSize = kernel.HalfSize;//(kernel.Size - 1) / 2;
            Color3f result = new Color3f();

            int startX = System.Math.Max(x - halfKernelSize, 0);
            int endX = System.Math.Min(x + halfKernelSize, bitmapData.Width - 1);

            int startY = System.Math.Max(y - halfKernelSize, 0);
            int endY = System.Math.Min(y + halfKernelSize, bitmapData.Height - 1);

            int kernelY = startY - (y - halfKernelSize);
            int kernelStartX = startX - (x - halfKernelSize);

            for (int yy = startY; yy <= endY; yy++)
            {
                int kernelX = kernelStartX;

                for (int xx = startX; xx <= endX; xx++)
                {
                    //result += (new Color3f(bitmap.GetPixel(xx, yy))) * kernel[kernelY, kernelX];

                    result += (new Color3f(scan0 + yy * bitmapData.Stride + xx * 3)) * kernel[kernelY, kernelX];

                    kernelX++;
                }

                kernelY++;
            }

            //bitmap.UnlockBits(bitmapData);

            return result;
        }

        /*
        public Color3f ApplyKernel(int x, int y)
        {
            return ConvolutionRenderer.ApplyKernel(_bitmap, _kernel, x, y);
        }*/

        public virtual Bitmap Render()
        {
            Bitmap result = new Bitmap(_bitmap.Width, _bitmap.Height);

            Rectangle rect = new Rectangle(0, 0, _bitmap.Width, _bitmap.Height);
            System.Drawing.Imaging.BitmapData bitmapData = _bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    Color3f color = ApplyKernel(bitmapData, _kernel, x, y);
                    result.SetPixel(x, y, color.ToColor());
                }
            }

            _bitmap.UnlockBits(bitmapData);

            return result;
        }
    }
}