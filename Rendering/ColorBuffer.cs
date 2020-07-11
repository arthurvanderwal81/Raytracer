using System.Drawing;

namespace Raytracer.Rendering
{
    public class ColorBuffer
    {
        public Bitmap Bitmap { get; set; }

        public ColorBuffer(int width, int height)
        {
            Bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }
    }
}
