using System.Drawing;
using Raytracer.Math;

namespace Raytracer.Textures
{
    public class BitmapTexture : ITexture
    {
        private Bitmap _bitmap;

        public BitmapTexture(string filename)
        {
            _bitmap = new Bitmap(filename);
        }
        public BitmapTexture(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public Color3f GetColor(Vector2d uvCoordinates, int MIPMAPLOD)
        {
            return new Color3f(_bitmap.GetPixel((int)(System.Math.Abs(uvCoordinates.X) * _bitmap.Width), (int)(System.Math.Abs(uvCoordinates.Y) * _bitmap.Height)));
        }
    }
}