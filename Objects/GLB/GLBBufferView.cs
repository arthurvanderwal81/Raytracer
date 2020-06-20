namespace Raytracer.Objects.GLB
{
    public class GLBBufferView
    {
        public enum Targets
        {
            ARRAY_BUFFER = 34962,
            ELEMENT_ARRAY_BUFFER = 34963
        }

        public byte[] Buffer { get; set; }
        public int ByteOffset { get; set; }
        public int ByteLength { get; set; }
        public int? Target { get; set; }
    }
}