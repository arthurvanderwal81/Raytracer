namespace Raytracer.Rendering
{
    public class RenderTarget
    {
        public ColorBuffer ColorBuffer { get; set; }
        public DepthBuffer DepthBuffer { get; set; }

        public RenderTarget(int width, int height)
        {
            ColorBuffer = new ColorBuffer(width, height);
            DepthBuffer = new DepthBuffer(width, height);
        }
    }
}