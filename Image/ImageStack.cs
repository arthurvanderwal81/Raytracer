using System.Drawing;
using System.Collections.Generic;

namespace Raytracer.Image
{
    public class ImageStack
    {
        public List<Bitmap> Bitmaps { get; set; }
        public Bitmap this[int imageIndex]
        {
            get
            {
                return Bitmaps[imageIndex];
            }
        }

        public ImageStack()
        {
            Bitmaps = new List<Bitmap>();
        }
    }
}