using Raytracer.Math;
using Raytracer.Textures;

namespace Raytracer.Materials
{
    public class Material
    {
        public Color3f Color { get; set; }
        public ITexture Texture { get; set; }

        public bool InvertNormals { get; set; }
        public bool FullBright { get; set; }
        public double Reflection { get; set; } // 0 = none, 1 = full
        public double Transparancy { get; set; } // 0 = solid, 1 = full transparant
    }
}