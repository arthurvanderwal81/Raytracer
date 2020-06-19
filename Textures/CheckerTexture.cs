using System.Drawing;
using Raytracer.Math;

namespace Raytracer.Textures
{
    public class CheckerTexture : ITexture
    {
        private Color3f[] _colors = new Color3f[]
        {
            new Color3f(Color.Blue),
            new Color3f(Color.White)
        };

        public int Width
        {
            get
            {
                return 50;
            }
        }

        public int Height
        {
            get
            {
                return 50;
            }
        }

        public CheckerTexture()
        {        
        }

        public CheckerTexture(Color color1, Color color2)
        {
            _colors = new Color3f[] { new Color3f(color1), new Color3f(color2) };
        }

        public Color3f GetColor(Vector2d uvCoordinates, int MIPMAPLOD)
        {        
            Color3f result = _colors[0];

            if (uvCoordinates.X < 0.0f)
            {
                uvCoordinates.X += 1.0f;
            }

            if (uvCoordinates.Y < 0.0f)
            {
                uvCoordinates.Y += 1.0f;
            }

            double u = uvCoordinates.X % 1f;
            double v = uvCoordinates.Y % 1f;

            if (u <= 0.5f && v <= 0.5f)
            {
                return _colors[1];
            }

            if (u > 0.5f && v > 0.5f)
            {
                return _colors[1];
            }

            return _colors[0];

            /*
            if ((((int)Math.Abs(textureCoordinates.X)) % 20) <= 10 && (((int)Math.Abs(textureCoordinates.Y)) % 20) <= 10)
            {
                result = _colors[1];
            }

            if ((((int)Math.Abs(textureCoordinates.X)) % 20) > 10 && (((int)Math.Abs(textureCoordinates.Y)) % 20) > 10)
            {
                result = _colors[1];
            }

            return result;
            */
        }
    }
}