namespace Raytracer.Math
{
    public unsafe class AverageColor3b
    {
        private int _r = 0;
        private int _g = 0;
        private int _b = 0;
        private int _samples = 0;

        public byte R
        {
            get
            {
                return (byte)(_r / _samples);
            }
        }

        public byte G
        {
            get
            {
                return (byte)(_g / _samples);
            }
        }

        public byte B
        {
            get
            {
                return (byte)(_b / _samples);
            }
        }

        public void Add(byte* pixelPointer)
        {
            _r += pixelPointer[2];
            _g += pixelPointer[1];
            _b += pixelPointer[0];

            _samples += 1;
        }
    }
}