using System;
using System.Drawing;
using System.Drawing.Imaging;

using Raytracer.Math;
using Raytracer.PostProcessing.Kernels;
using Raytracer.Rendering;

namespace Raytracer.PostProcessing
{
    public class DepthBlurRenderer
    {
        private RenderTarget _renderTarget;
        private double _focalLength;
        private double _focalLengthMin;
        private double _focalLengthMax;
        private int _maxKernelSize;
        //private int _halfKernelSize;

        public DepthBlurRenderer(RenderTarget renderTarget, double focalLengthMin, double focalLengthMax, int maxKernelSize = 21)
        {
            if ((maxKernelSize % 2) != 1)
            {
                throw new ArgumentException("Kernelsize has to be an odd number");
            }

            _renderTarget = renderTarget;
            _focalLength = (focalLengthMin + focalLengthMax) / 2;
            _focalLengthMin = focalLengthMin;
            _focalLengthMax = focalLengthMax;
            _maxKernelSize = maxKernelSize;
            //_halfKernelSize = (_kernelSize - 1) / 2;
        }

        public unsafe Bitmap Render()
        {
            int width = _renderTarget.ColorBuffer.Bitmap.Width;
            int height = _renderTarget.ColorBuffer.Bitmap.Height;

            Rectangle imageRectangle = new Rectangle(new Point { X = 0, Y = 0 }, _renderTarget.ColorBuffer.Bitmap.Size);
            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bitmapData = _renderTarget.ColorBuffer.Bitmap.LockBits(imageRectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData resultBitmapData = result.LockBits(imageRectangle, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            byte* bitmapScan0 = (byte*)bitmapData.Scan0.ToPointer();
            byte* resultScan0 = (byte*)resultBitmapData.Scan0.ToPointer();

            byte* bitmapPixelPointer;
            byte* resultPixelPointer;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double pixelDepth = _renderTarget.DepthBuffer[y, x];

                    bitmapPixelPointer = bitmapScan0 + y * bitmapData.Stride + x * 3;
                    resultPixelPointer = resultScan0 + y * resultBitmapData.Stride + x * 3;

                    if (pixelDepth >= _focalLengthMin && pixelDepth <= _focalLengthMax)
                    {
                        resultPixelPointer[0] = bitmapPixelPointer[0];
                        resultPixelPointer[1] = bitmapPixelPointer[1];
                        resultPixelPointer[2] = bitmapPixelPointer[2];
                    }
                    else
                    {
                        AverageColor3b color = new AverageColor3b();

                        int kernelSize = (int)(ScalarHelpers.Saturate(System.Math.Abs(pixelDepth - _focalLength) / 8.0) * _maxKernelSize);

                        if ((kernelSize % 2) == 0)
                        {
                            if (kernelSize > 0)
                            {
                                kernelSize = kernelSize - 1;
                            }
                            else
                            {
                                kernelSize = 1;
                            }
                        }

                        int halfKernelSize = (kernelSize - 1) / 2;

                        Rectangle kernelRectangle = new Rectangle(x - halfKernelSize, y - halfKernelSize, kernelSize, kernelSize);
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
            }

            _renderTarget.ColorBuffer.Bitmap.UnlockBits(bitmapData);
            result.UnlockBits(resultBitmapData);

            return result;
        }
    }
}