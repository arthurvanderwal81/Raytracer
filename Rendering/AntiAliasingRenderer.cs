using System;
using System.Drawing;

namespace Raytracer.Rendering
{
    public class AntiAliasingRenderer
    {
        private Bitmap _bitmap;
        public AntiAliasingRenderer(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public Bitmap Render(int factor)
        {
            if (_bitmap.Width % factor != 0)
            {
                throw new Exception("Invalid factor");
            }

            if (_bitmap.Height % factor != 0)
            {
                throw new Exception("Invalid factor");
            }

            int resultWidth = _bitmap.Width / factor;
            int resultHeight = _bitmap.Height / factor;

            AverageColorBitmap averageColorBitmap = new AverageColorBitmap(resultWidth, resultHeight);

            for (int y = 0; y < _bitmap.Height; y++)
            {
                int mappedY = y / factor;

                for (int x = 0; x < _bitmap.Width; x++)
                {
                    int mappedX = x / factor;

                    averageColorBitmap.Add(mappedX, mappedY, _bitmap.GetPixel(x, y));
                }
            }

            return averageColorBitmap.ToBitmap();
        }
    }
}