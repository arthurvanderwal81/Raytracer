using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Raytracer.Rendering
{
    public class AverageColor
    {
        List<Color> _colors;

        public Color Color
        {
            get
            {
                int averageR = 0;
                int averageG = 0;
                int averageB = 0;

                foreach (Color color in _colors)
                {
                    averageR += color.R;
                    averageG += color.G;
                    averageB += color.B;
                }

                return Color.FromArgb(averageR / _colors.Count, averageG / _colors.Count, averageB / _colors.Count);
            }
        }

        public AverageColor()
        {
            _colors = new List<Color>();
        }

        public bool Add(Color color)
        {
            bool result = _colors.Count != 0 && _colors.Any(c => c != color);

            _colors.Add(color);

            return result;
        }
    }
}