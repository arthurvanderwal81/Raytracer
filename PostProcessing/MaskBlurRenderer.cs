﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using Raytracer.Math;

namespace raytracer.PostProcessing
{
    public class MaskBlurRenderer
    {
        private Bitmap _bitmap;
        private Bitmap _mask;
        private int _kernelSize;
        private int _halfKernelSize;

        public MaskBlurRenderer(Bitmap bitmap, Bitmap mask, int kernelSize = 9)
        {
            if ((kernelSize % 2) != 1)
            {
                throw new ArgumentException("Kernelsize has to be an odd number");
            }

            if (bitmap.Width != mask.Width || bitmap.Height != mask.Height)
            {
                throw new ArgumentException("Bitmap and mask must have the same dimensions");
            }

            _bitmap = bitmap;
            _mask = mask;
            _kernelSize = kernelSize;
            _halfKernelSize = (_kernelSize - 1) / 2;
        }

        public unsafe Bitmap Render()
        {
            int width = _bitmap.Width;
            int height = _bitmap.Height;

            Rectangle imageRectangle = new Rectangle(new Point { X = 0, Y = 0 }, _bitmap.Size);
            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bitmapData = _bitmap.LockBits(imageRectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData maskBitmapData = _mask.LockBits(imageRectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData resultBitmapData = result.LockBits(imageRectangle, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            byte* bitmapScan0 = (byte*)bitmapData.Scan0.ToPointer();
            byte* maskScan0 = (byte*)maskBitmapData.Scan0.ToPointer();
            byte* resultScan0 = (byte*)resultBitmapData.Scan0.ToPointer();

            byte* bitmapPixelPointer;
            byte* maskPixelPointer;
            byte* resultPixelPointer;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maskPixelPointer = maskScan0 + y * maskBitmapData.Stride + x * 3;
                    resultPixelPointer = resultScan0 + y * resultBitmapData.Stride + x * 3;

                    if (maskPixelPointer[0] == 255 && maskPixelPointer[1] == 255 && maskPixelPointer[2] == 255)
                    {
                        bitmapPixelPointer = bitmapScan0 + y * bitmapData.Stride + x * 3;

                        resultPixelPointer[0] = bitmapPixelPointer[0];
                        resultPixelPointer[1] = bitmapPixelPointer[1];
                        resultPixelPointer[2] = bitmapPixelPointer[2];
                    }
                    else
                    {
                        AverageColor3b color = new AverageColor3b();

                        Rectangle kernelRectangle = new Rectangle(x - _halfKernelSize, y - _halfKernelSize, _kernelSize, _kernelSize);
                        Rectangle intersection = Rectangle.Intersect(imageRectangle, kernelRectangle);

                        for (int yy = intersection.Y; yy < intersection.Bottom; yy++)
                        {
                            for (int xx = intersection.X; xx < intersection.Right; xx++)
                            {
                                bitmapPixelPointer = bitmapScan0 + yy * bitmapData.Stride + xx * 3;
                                maskPixelPointer = maskScan0 + yy * maskBitmapData.Stride + xx * 3;

                                if (maskPixelPointer[0] == 255 && maskPixelPointer[1] == 255 && maskPixelPointer[2] == 255)
                                {
                                    continue;
                                }

                                color.Add(bitmapPixelPointer);
                            }
                        }

                        resultPixelPointer[0] = color.B;
                        resultPixelPointer[1] = color.G;
                        resultPixelPointer[2] = color.R;
                    }
                }
            }

            _bitmap.UnlockBits(bitmapData);
            _mask.UnlockBits(maskBitmapData);
            result.UnlockBits(resultBitmapData);

            return result;
        }
    }
}