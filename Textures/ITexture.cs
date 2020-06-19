using Raytracer.Math;

namespace Raytracer.Textures
{
    public interface ITexture
    {
        Color3f GetColor(Vector2d uvCoordinates, int MIPMAPLOD);
    }
}