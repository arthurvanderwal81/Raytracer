namespace Raytracer.Math
{
    public static class ScalarHelpers
    {
        public static double Saturate(double x)
        {
            return System.Math.Min(1f, System.Math.Max(0f, x));
        }
    }
}