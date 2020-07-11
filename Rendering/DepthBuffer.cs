using System.Drawing;
using System.Drawing.Imaging;

namespace Raytracer.Rendering
{
    public class DepthBuffer
    {
        private double[,] _values { get; set; }
        private double _maxValue;

        public DepthBuffer(int width, int height)
        {
            _values = new double[height, width];
            _maxValue = double.MinValue;
        }

        public Bitmap ToBitmap()
        {
            int width = _values.GetLength(1);
            int height = _values.GetLength(0);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int colorChannel = (int)(Math.ScalarHelpers.Saturate(_values[y, x] / 10.0) * 255.0);
                    result.SetPixel(x, y, Color.FromArgb(colorChannel, colorChannel, colorChannel));
                }
            }

            return result;
        }

        public double this[int y, int x]
        {
            get
            {
                return _values[y, x];
            }
            set
            {
                _values[y, x] = value;

                if (value > _maxValue)
                {
                    _maxValue = value;
                }
            }
        }
    }
}