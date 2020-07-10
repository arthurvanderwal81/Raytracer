using System;
using System.Drawing;
using System.Threading;

using Raytracer.Math;
using Raytracer.Rendering.Intersection;

namespace Raytracer.Rendering
{
    public class Raytracer 
    {
        public delegate void ProgressDelegate(float progress);
        public delegate void RenderCompleteDelegate(RenderTarget result);

        public ProgressDelegate RenderProgress;
        public RenderCompleteDelegate RenderComplete;

        private unsafe class RaytracerData
        {
            public int X0 { get; set; }
            public int X1 { get; set; }
            public int Y { get; set; }
            public Camera Camera { get; set; }
            public Scene Scene { get; set; }
            public byte* ScanLine { get; set; }
            public DepthBuffer DepthBuffer { get; set; }
        }

        private int _screenResolutionX;
        private int _screenResolutionY;

        private bool _renderDOF;

        public Scene Scene { get; private set; }

        public Camera Camera { get; private set; }

        public Raytracer(int screenResolutionX, int screenResolutionY, bool renderDOF = true)
        {
            _screenResolutionX = screenResolutionX;
            _screenResolutionY = screenResolutionY;
            _renderDOF = renderDOF;

            Scene = new Scene();
            Camera = new Camera(_screenResolutionX, _screenResolutionY, 1.2f);//2.2f);// _screenResolutionX / _screenResolutionY); //1.2f);
        }

        public Raytracer(Size resolution, bool renderDOF) : this(resolution.Width, resolution.Height, renderDOF)
        {
        }

        private static unsafe void RenderScanLineDOF(object data)
        {
            RaytracerData raytracerData = data as RaytracerData;

            Random random = new Random();

            for (int x = raytracerData.X0; x < raytracerData.X1; x++)
            {
                AverageColor averageColor = new AverageColor();

                Vector3d focalPoint = raytracerData.Camera.Position + raytracerData.Camera.GetRayDirection(x, raytracerData.Y) * raytracerData.Camera.FocalLength;

                Vector3d ws0 = raytracerData.Camera.ScreenSpaceToWorldSpace(x - 5, raytracerData.Y - 5);
                Vector3d ws1 = raytracerData.Camera.ScreenSpaceToWorldSpace(x + 5, raytracerData.Y + 5);
                //Vector3d ws0 = raytracerData.Camera.ScreenSpaceToWorldSpace(x, raytracerData.Y);
                //Vector3d ws1 = raytracerData.Camera.ScreenSpaceToWorldSpace(x, raytracerData.Y);

                Vector3d range = ws1 - ws0;

                /*
                for (int i = 0; i < 32; i++)
                {
                    double rand = (double)random.NextDouble();
                    Vector3d randomPixelWorldSpace = ws0 + rand * range;
                    Vector3d rayDirection = (focalPoint - randomPixelWorldSpace).Normalize();

                    IntersectionResult intersectionResult = raytracerData.Scene.GetNearestObjectIntersection(rayDirection, randomPixelWorldSpace);

                    Color color = Color.Black;

                    if (intersectionResult.Object != null)
                    {
                        color = intersectionResult.Object.GetColor(rayDirection, intersectionResult.Intersection, raytracerData.Scene);
                    }

                    bool a = averageColor.Add(color);

                    if (a)
                    {
                        a = false;
                    }
                }
                */

                for (int yy = raytracerData.Y - 3; yy < raytracerData.Y + 3; yy++)
                {
                    for (int xx = x - 3; xx < x + 3; xx++)
                    {
                        Vector3d pixelWorldSpace = raytracerData.Camera.ScreenSpaceToWorldSpace(xx, yy);
                        Vector3d rayDirection = (focalPoint - pixelWorldSpace).Normalize();

                        //Vector3d raydirection = raytracerData.Camera.GetRayDirection(x, raytracerData.Y);
                        //Vector3d rayposition = raytracerData.Camera.Position;

                        IIntersectionResult intersectionResult = raytracerData.Scene.GetNearestObjectIntersection(rayDirection, pixelWorldSpace);

                        Color color = Color.Black;

                        if (intersectionResult != null)
                        {
                            color = intersectionResult.Object.GetColor(rayDirection, intersectionResult, raytracerData.Scene);
                        }

                        averageColor.Add(color);
                    }
                }
                

                Color finalColor = averageColor.Color;

                //result.SetPixel(x, y, color);
                byte* pixelPointer = raytracerData.ScanLine + x * 3;

                pixelPointer[0] = finalColor.B;
                pixelPointer[1] = finalColor.G;
                pixelPointer[2] = finalColor.R;
            }
        }

        private static unsafe void RenderScanLine(object data)
        {
            RaytracerData raytracerData = data as RaytracerData;

            for (int x = raytracerData.X0; x < raytracerData.X1; x++)
            {
                //x = 799;
                //raytracerData.Y = 655;
                Vector3d raydirection = raytracerData.Camera.GetRayDirection(x, raytracerData.Y);
                Vector3d rayposition = raytracerData.Camera.Position;

                IIntersectionResult intersectionResult = raytracerData.Scene.GetNearestObjectIntersection(raydirection, rayposition);

                Color color = Color.Black;

                if (intersectionResult != null)
                {
                    color = intersectionResult.Object.GetColor(raydirection, intersectionResult, raytracerData.Scene);

                    double z = raytracerData.Camera.Direction.Dot(intersectionResult.Intersection);
                    raytracerData.DepthBuffer[raytracerData.Y, x] = z;
                }

                byte* pixelPointer = raytracerData.ScanLine + x * 3;

                pixelPointer[0] = color.B;
                pixelPointer[1] = color.G;
                pixelPointer[2] = color.R;
            }
        }

        public unsafe void Render()
        {
            Scene.UpdateVisibleObjects(Camera);

            RenderTarget result = new RenderTarget(_screenResolutionX, _screenResolutionY);

            //Bitmap result = new Bitmap(_screenResolutionX, _screenResolutionY, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Rectangle rect = new Rectangle(0, 0, result.ColorBuffer.Bitmap.Width, result.ColorBuffer.Bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = result.ColorBuffer.Bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, result.ColorBuffer.Bitmap.PixelFormat);

            byte* scan0 = (byte*)bmpData.Scan0.ToPointer();

            for (int y = 0; y < _screenResolutionY; y++)
            {
                byte* scanLine = scan0 + y * bmpData.Stride;

                RaytracerData raytracerData = new RaytracerData()
                {
                    X0 = 0,
                    X1 = _screenResolutionX,
                    Y = y,
                    Camera = Camera,
                    Scene = Scene,
                    ScanLine = scanLine,
                    DepthBuffer = result.DepthBuffer
                };

                if (_renderDOF)
                {
                    RenderScanLineDOF(raytracerData);
                }
                else
                {
                    RenderScanLine(raytracerData);
                }

                RenderProgress?.Invoke((float)y / (float)_screenResolutionY);
            }

            result.ColorBuffer.Bitmap.UnlockBits(bmpData);

            RenderComplete?.Invoke(result);
        }
    }
}