using System;
using System.Drawing;
using System.Drawing.Imaging;

using Raytracer.Math;

namespace Raytracer.PostProcessing
{
     public class BoxBlurRenderer
    {
        private Bitmap _bitmap;
        private int _kernelSize;
        private int _halfKernelSize;

        public BoxBlurRenderer(Bitmap bitmap, int kernelSize)
        {
            if ((kernelSize % 2) != 1)
            {
                throw new ArgumentException("Kernelsize has to be an odd number");
            }

            _bitmap = bitmap;
            _kernelSize = kernelSize;
            _halfKernelSize = (_kernelSize - 1) / 2;
        }

        public BoxBlurRenderer(string filename, int kernelSize) : this(new Bitmap(filename), kernelSize)
        {
        }

        public unsafe Bitmap Render()
        {
            int width = _bitmap.Width;
            int height = _bitmap.Height;

            Rectangle imageRectangle = new Rectangle(new Point { X = 0, Y = 0 }, _bitmap.Size);
            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bitmapData = _bitmap.LockBits(imageRectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData resultBitmapData = result.LockBits(imageRectangle, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            byte* bitmapScan0 = (byte*)bitmapData.Scan0.ToPointer();
            byte* resultScan0 = (byte*)resultBitmapData.Scan0.ToPointer();

            byte* bitmapPixelPointer;
            byte* resultPixelPointer;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    resultPixelPointer = resultScan0 + y * resultBitmapData.Stride + x * 3;

                    AverageColor3b color = new AverageColor3b();

                    Rectangle kernelRectangle = new Rectangle(x - _halfKernelSize, y - _halfKernelSize, _kernelSize, _kernelSize);
                    Rectangle intersection = Rectangle.Intersect(imageRectangle, kernelRectangle);

                    for (int yy = intersection.Y; yy < intersection.Bottom; yy++)
                    {
                        for (int xx = intersection.X; xx < intersection.Right; xx++)
                        {
                            bitmapPixelPointer = bitmapScan0 + yy * bitmapData.Stride + xx * 3;

                            color.Add(bitmapPixelPointer);
                        }
                    }

                    resultPixelPointer[0] = color.B;
                    resultPixelPointer[1] = color.G;
                    resultPixelPointer[2] = color.R;
                }
            }

            _bitmap.UnlockBits(bitmapData);
            result.UnlockBits(resultBitmapData);

            return result;
        }
    }
}