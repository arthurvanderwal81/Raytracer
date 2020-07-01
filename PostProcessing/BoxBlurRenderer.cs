using System.Drawing;

namespace Raytracer.PostProcessing
{
    public class BoxBlurRenderer : ConvolutionRenderer
    {
        public BoxBlurRenderer(Bitmap bitmap, int kernelSize) : base(bitmap)
        {
            _kernel = new Kernel(kernelSize);

            float kernelSizeSquared = (float)(kernelSize * kernelSize);

            for (int y = 0; y < kernelSize; y++)
            {
                for (int x = 0; x < kernelSize; x++)
                {
                    _kernel[y, x] = 1.0f / kernelSizeSquared;
                }
            }
        }

        public BoxBlurRenderer(string filename, int kernelSize) : this(new Bitmap(filename), kernelSize)
        {
        }
    }
}