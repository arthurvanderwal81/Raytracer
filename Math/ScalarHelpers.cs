namespace Raytracer.Math
{
    public static class ScalarHelpers
    {
        public static double Saturate(double x)
        {
            return System.Math.Min(1f, System.Math.Max(0f, x));
        }

        public static int Round(double x)
        {
            return (int)(x + 0.5);
        }
    }
}