using System.Drawing;
using System.Drawing.Imaging;

namespace Raytracer.PostProcessing.Kernels
{
    public class AverageKernel : Kernel
    {
        public AverageKernel(int size) : base(size)
        {
        }

        public override KernelResult Apply(BitmapData bitmapData, int x, int y, Rectangle rectangle)
        {
            KernelResult result = base.Apply(bitmapData, x, y, rectangle);

            result.Value /= result.Rectangle.Width * result.Rectangle.Height;

            return result;
        }
    }
}