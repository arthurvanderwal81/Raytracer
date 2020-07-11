using System.Drawing;

using Raytracer.Math;
using Raytracer.PostProcessing.Kernels;

namespace Raytracer.PostProcessing
{
    // TODO: rewrite to not use convolutionrenderer
    public class RadialBlurRenderer : ConvolutionRenderer
    {
        private float _nonBlurRadius;
        private Vector2d _bitmapCenter;

        public RadialBlurRenderer(Bitmap bitmap) : base(bitmap)
        {
            _bitmapCenter = new Vector2d(bitmap.Width / 2.0f, bitmap.Height / 2.0f);
        }

        public RadialBlurRenderer(string filename, int kernelSize, int nonBlurRadius) : this(new Bitmap(filename))
        {
            _kernel = new Kernel(kernelSize);
            _nonBlurRadius = nonBlurRadius;
        }

        private void CalculateKernel(int x, int y)
        {
            _kernel = new Kernel(_kernel.Size);
            
            Vector2d currentPosition = _kernel.GetCenter();

            Vector2d distanceVector = (new Vector2d(x, y) - _bitmapCenter);
            double distance = distanceVector.Length();

            if (distance <= _nonBlurRadius)
            {
                _kernel[currentPosition] = 1.0f;
                return;
            }

            Vector2d direction = distanceVector / distance;

            int count = 0;
            _kernel = new Kernel(_kernel.Size);

            while (System.Math.Round(currentPosition.X) >= 0 && 
                   System.Math.Round(currentPosition.X) < _kernel.Size &&
                   System.Math.Round(currentPosition.Y) >= 0 && 
                   System.Math.Round(currentPosition.Y) < _kernel.Size)
            {
                if (_kernel[currentPosition] != 1.0f)
                {
                    _kernel[currentPosition] = 1.0f;
                    count++;
                }

                currentPosition += direction;
            }

            currentPosition = _kernel.GetCenter();

            while (System.Math.Round(currentPosition.X) >= 0 &&
                   System.Math.Round(currentPosition.X) < _kernel.Size &&
                   System.Math.Round(currentPosition.Y) >= 0 &&
                   System.Math.Round(currentPosition.Y) < _kernel.Size)
            {
                if (_kernel[currentPosition] != 1.0f)
                {
                    _kernel[currentPosition] = 1.0f;
                    count++;
                }

                currentPosition -= direction;
            }

            _kernel = _kernel / (float)count;
        }

        public override Bitmap Render()
        {
            Bitmap result = new Bitmap(_bitmap.Width, _bitmap.Height);

            Rectangle rect = new Rectangle(0, 0, _bitmap.Width, _bitmap.Height);
            System.Drawing.Imaging.BitmapData bitmapData = _bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            //CalculateKernel(145, 128);

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    CalculateKernel(x, y);
                    Color3f color = ApplyKernel(bitmapData, _kernel, x, y);
                    result.SetPixel(x, y, color.ToColor());
                }
            }

            _bitmap.UnlockBits(bitmapData);

            return result;
        }
    }
}