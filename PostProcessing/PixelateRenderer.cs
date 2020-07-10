using Raytracer.Rendering;
using System.Drawing;

namespace Raytracer.PostProcessing
{
    public class PixelateRenderer
    {
        private Bitmap _bitmap;
        private Rectangle _region;
        private Size _pixelSize;

        public PixelateRenderer(Bitmap bitmap, Rectangle region, Size pixels)
        {
            _bitmap = bitmap;
            _region = region;
            _pixelSize = new Size(region.Width / pixels.Width, region.Height / pixels.Height);
        }

        public PixelateRenderer(string filename, Rectangle region, Size pixels) : this(new Bitmap(filename), region, pixels)
        {
        }

        public Bitmap Render1()
        {
            Bitmap result = new Bitmap(_bitmap.Width, _bitmap.Height);
            AverageColor[,] pixels = new AverageColor[_region.Height / _pixelSize.Height, _region.Width / _pixelSize.Width];

            for (int y = 0; y < _bitmap.Height; y++)
            {
                for (int x = 0; x < _bitmap.Width; x++)
                {
                    if (_region.Contains(x, y))
                    {
                        int pixelX = (x - _region.X) / _pixelSize.Width;
                        int pixelY = (y - _region.Y) / _pixelSize.Height;

                        if (pixels[pixelY, pixelX] == null)
                        {
                            pixels[pixelY, pixelX] = new AverageColor();
                        }

                        pixels[pixelY, pixelX].Add(_bitmap.GetPixel(x, y));
                    }
                    else
                    {
                        result.SetPixel(x, y, _bitmap.GetPixel(x, y));
                    }
                }
            }

            for (int y = _region.Y; y < _region.Bottom; y++)
            {
                for (int x = _region.X; x < _region.Right; x++)
                {
                    int pixelX = (x - _region.X) / _pixelSize.Width;
                    int pixelY = (y - _region.Y) / _pixelSize.Height;

                    result.SetPixel(x, y, pixels[pixelY, pixelX].Color);
                }
            }

            return result;
        }

        public Bitmap Render()
        {
            Bitmap result = (Bitmap)_bitmap.Clone();
            AverageColor[,] pixels = new AverageColor[_region.Height / _pixelSize.Height, _region.Width / _pixelSize.Width];

            for (int y = _region.Y; y < _region.Bottom; y++)
            {
                for (int x = _region.X; x < _region.Right; x++)
                {
                    int pixelX = (x - _region.X) / _pixelSize.Width;
                    int pixelY = (y - _region.Y) / _pixelSize.Height;

                    if (pixels[pixelY, pixelX] == null)
                    {
                        pixels[pixelY, pixelX] = new AverageColor();
                    }

                    pixels[pixelY, pixelX].Add(_bitmap.GetPixel(x, y));

                }
            }

            for (int y = _region.Y; y < _region.Bottom; y++)
            {
                for (int x = _region.X; x < _region.Right; x++)
                {
                    int pixelX = (x - _region.X) / _pixelSize.Width;
                    int pixelY = (y - _region.Y) / _pixelSize.Height;

                    result.SetPixel(x, y, pixels[pixelY, pixelX].Color);
                }
            }

            return result;
        }
    }
}