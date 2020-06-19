using System.Drawing;

namespace Raytracer.Rendering
{
    public class AverageColorBitmap
    {
        private int _width;
        private int _height;
        private AverageColor[,] _averageColors;

        public AverageColorBitmap(int width, int height)
        {
            _width = width;
            _height = height;

            _averageColors = new AverageColor[height, width];
        }

        public void Add(int x, int y, Color color)
        {
            if (_averageColors[y, x] == null)
            {
                _averageColors[y, x] = new AverageColor();
            }

            _averageColors[y, x].Add(color);
        }

        public Bitmap ToBitmap()
        {
            Bitmap result = new Bitmap(_width, _height);

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    result.SetPixel(x, y, _averageColors[y, x].Color);
                }
            }

            return result;

        }
    }
}