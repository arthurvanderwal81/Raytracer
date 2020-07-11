using System.Drawing;

namespace Raytracer.PostProcessing.Kernels
{
    public class KernelResult
    {
        public Rectangle Rectangle { get; set; }
        public float Value { get; set; }
    }
}