using System;
using System.Drawing;
using System.Drawing.Imaging;

using Raytracer.Rendering;

namespace Raytracer.PostProcessing
{
    public class DepthBlurRenderer
    {
        private RenderTarget _renderTarget;
        private double _focalLengthMin;
        private double _focalLengthMax;
        private int _kernelSize;
        private int _halfKernelSize;

        public DepthBlurRenderer(RenderTarget renderTarget, double focalLengthMin, double focalLengthMax, int kernelSize = 9)
        {
            if ((kernelSize % 2) != 1)
            {
                throw new ArgumentException("Kernelsize has to be an odd number");
            }

            _renderTarget = renderTarget;
            _focalLengthMin = focalLengthMin;
            _focalLengthMax = focalLengthMax;
            _kernelSize = kernelSize;
            _halfKernelSize = (_kernelSize - 1) / 2;
        }

        public Bitmap Render()
        {
            Rectangle imageRectangle = new Rectangle(new Point { X = 0, Y = 0 }, _renderTarget.ColorBuffer.Bitmap.Size);
            int width = _renderTarget.ColorBuffer.Bitmap.Width;
            int height = _renderTarget.ColorBuffer.Bitmap.Height;

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double pixelDepth = _renderTarget.DepthBuffer[y, x];

                    if (pixelDepth >= _focalLengthMin && pixelDepth <= _focalLengthMax)
                    {
                        result.SetPixel(x, y, _renderTarget.ColorBuffer.Bitmap.GetPixel(x, y));
                    }
                    else
                    {
                        AverageColor color = new AverageColor();

                        Rectangle kernelRectangle = new Rectangle(x - _halfKernelSize, y - _halfKernelSize, _kernelSize, _kernelSize);
                        Rectangle intersection = Rectangle.Intersect(imageRectangle, kernelRectangle);

                        for (int yy = intersection.Y; yy < intersection.Bottom; yy++)
                        {
                            for (int xx = intersection.X; xx < intersection.Right; xx++)
                            {
                                // TODO: check if pixeldepth outside focaldepth bounds
                                color.Add(_renderTarget.ColorBuffer.Bitmap.GetPixel(xx, yy));
                            }
                        }

                        result.SetPixel(x, y, color.Color);
                    }
                }
            }

            return result;
        }
    }
}