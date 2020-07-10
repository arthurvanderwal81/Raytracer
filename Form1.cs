using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

using Raytracer.Math;
using Raytracer.Lights;
using Raytracer.Objects;
using Raytracer.Textures;
using Raytracer.Materials;
using Raytracer.Objects.GLB;
using Raytracer.PostProcessing;
using Raytracer.Rendering;
using Raytracer.Image;

namespace Raytracer
{
    public partial class Form1 : Form
    {
        private Bitmap _raytracerOutput = null;
        private RenderTarget _raytracerResult = null;
        private Animation _animation = new Animation();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            MouseDown += Form1MouseDown;

            //PixelateRenderer pixelate = new PixelateRenderer(@"..\..\Data\Textures\10531.people.jpg", new Rectangle(235, 87, 50, 50), new Size(5, 5));
            //_raytracerOutput = pixelate.Render();

            //RadialBlurRenderer radialBlurRenderer = new RadialBlurRenderer(@"..\..\Data\Textures\fp695.png", 21, 20);
            //RadialBlurRenderer radialBlurRenderer = new RadialBlurRenderer(@"..\..\Data\Textures\IMG_20200618_191644.jpg", 21, 20);            
            //_raytracerOutput = radialBlurRenderer.Render();

            //_raytracerOutput.Save(string.Format(@"..\..\Data\Results\{0}.bmp", Environment.TickCount));

            //BoxBlurRenderer boxBlurRenderer = new BoxBlurRenderer(@"..\..\Data\Textures\fp695.png", 5);
            //_raytracerOutput = boxBlurRenderer.Render();
            
            // TODO:
            // https://github.com/KhronosGroup/glTF/tree/master/specification/2.0
            // Create inverse cube object
            // Add transparant surfaces
            // Add simple scene file format - JSON
            // Add scene viewer & editor
            // Reflection of reflection not working (flipped)
            // Infinite reflection fix
            // Soft shadows
            // MIPMAPPING
            // Bounding sphere light problem

            int renderResultWidth = 1024;
            int renderResultHeight = 768;

            this.ClientSize = new Size(renderResultWidth, renderResultHeight + progressBar1.Height);
            Rendering.Raytracer raytracer = new Rendering.Raytracer(renderResultWidth, renderResultHeight, false);
            //raytracer.Camera.FocalLength = 1.5;

            //Raytracer raytracer = new Raytracer(this.ClientSize, false);
            //Rendering.Raytracer raytracer = new Rendering.Raytracer(1024, 768, false);
            //Rendering.Raytracer raytracer = new Rendering.Raytracer(3840, 2160, false);
            //Raytracer raytracer = new Raytracer(7680, 4320);

            //raytracer.Camera.Position = new Vector3d(5f, 5f, 2f);
            //raytracer.Camera.LookAt(new Vector3d(10d, 2d, -1.5d));

            raytracer.Camera.Position = new Vector3d(5f, 5f, -2f);
            raytracer.Camera.LookAt(new Vector3d(10d, 2d, 1.5d));

            //raytracer.Camera.Position = new Vector3d(60d, 60d, 60d);
            //raytracer.Camera.LookAt(new Vector3d(0d, 0d, 0d));

            Vector3d focalPoint = raytracer.Camera.Position + raytracer.Camera.GetRayDirection(1024/2, 768/2) * raytracer.Camera.FocalLength;

            Material redMaterial = new Material();
            redMaterial.Color = new Color3f(Color.Red);

            Material greenMaterial = new Material();
            greenMaterial.Color = new Color3f(Color.Green);

            Material blueMaterial = new Material();
            blueMaterial.Color = new Color3f(Color.LightBlue);
            blueMaterial.Reflection = 0.1f;

            Material reflectiveMaterial = new Material();
            reflectiveMaterial.Color = new Color3f(.7, .7, .7);
            reflectiveMaterial.Reflection = .7f;

            Material semiReflectiveMaterial = new Material();
            semiReflectiveMaterial.Reflection = 0.5f;
            semiReflectiveMaterial.Texture = new CheckerTexture();
            semiReflectiveMaterial.Color = new Color3f(Color.Yellow);

            Material checkerMaterial = new Material();
            checkerMaterial.Texture = new CheckerTexture();

            Material greenCheckerMaterial = new Material();
            greenCheckerMaterial.Texture = new CheckerTexture(Color.White, Color.Green);

            Material marbleMaterial = new Material();
            marbleMaterial.Texture = new BitmapTexture(@"..\..\Data\Textures\pf-bg12-ake5622-2-ake.jpg");

            Material woodMaterial = new Material();
            woodMaterial.Texture = new BitmapTexture(@"..\..\Data\Textures\free-wood-texture-with-high-resolution.jpg");

            Material backgroundMaterial1 = new Material();
            backgroundMaterial1.Texture = new BitmapTexture(@"..\..\Data\Textures\IMG_20200618_191644.jpg");

            List<Material> testMaterials = new List<Material>();

            Color[] colors = new Color[]
            {
                Color.Blue,
                Color.Beige,
                Color.Gray,
                Color.Green,
                Color.Red,
                Color.Pink,
                Color.Purple,
                Color.Aqua,
                Color.Yellow,
                Color.White,
                Color.Cyan,
                Color.HotPink
            };

            foreach (Color color in colors)
            {
                Material material = new Material();
                material.Color = new Color3f(color);

                testMaterials.Add(material);
            }

            /*
            GLBFileData gblFileData = GLBFileReader.ReadFile(@"..\..\Data\Objects\Pig.glb");
            raytracer.Scene.Objects.AddRange(gblFileData.Meshes[0].GetTriangleObjects(0));

            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(49d, 49d, 49d)));
            */

            //raytracer.Scene.Objects.Add(new SphereObject("SPHERE_RIGHT",    new Vector3d(10f, 2f, 3f), 1f, woodMaterial));
            raytracer.Scene.Objects.Add(new SphereObject("SPHERE_MIDDLE",   new Vector3d(10f, 2f, 0f), 1f, reflectiveMaterial));
            raytracer.Scene.Objects.Add(new CubeObject("CUBE_RIGHT", new Vector3d(10f, 2f, 3f), 2f, woodMaterial));
            raytracer.Scene.Objects.Add(new SphereObject("SPHERE_LEFT",     new Vector3d(10f, 2f, -3f), 1f, blueMaterial));

            raytracer.Scene.Objects.Add(new SphereObject("SPHERE_FOCUS", raytracer.Camera.Position + raytracer.Camera.Direction * raytracer.Camera.FocalLength, 0.15f, reflectiveMaterial));

            //raytracer.Scene.Objects.Add(new TriangleObject("VERTICAL_TRIANGLE_1", new Vertex(12f, 1f, -10f, 0f, 1f), new Vertex(12f, 1f, 10f, 1f, 1f), new Vertex(12f, 16f, 10f, 1f, 0f), backgroundMaterial1));
            //raytracer.Scene.Objects.Add(new TriangleObject("VERTICAL_TRIANGLE_2", new Vertex(12f, 16f, 10f, 1f, 0f), new Vertex(12f, 16f, -10f, 0f, 0f), new Vertex(12f, 1f, -10f, 0f, 1f), backgroundMaterial1));

            raytracer.Scene.Objects.Add(new QuadObject("VERTICAL_QUAD", new Vertex(12f, 1f, -10f, 0f, 1f), new Vertex(12f, 1f, 10f, 1f, 1f), new Vertex(12f, 16f, 10f, 1f, 0f), new Vertex(12f, 16f, -10f, 0f, 0f), backgroundMaterial1));

            //raytracer.Scene.Objects.Add(new TriangleObject("FLOOR_TRIANGLE_1", new Vertex(1f, 1f, -5f, 0f, 0f), new Vertex(1f, 1f, 5f, 2f, 0f), new Vertex(15f, 1f, 5f, 2f, 6f), checkerMaterial));
            //raytracer.Scene.Objects.Add(new TriangleObject("FLOOR_TRIANGLE_2", new Vertex(15f, 1f, 5f, 2f, 6f), new Vertex(15f, 1f, -5f, 0f, 6f), new Vertex(1f, 1f, -5f, 0f, 0f), checkerMaterial));

            raytracer.Scene.Objects.Add(new PlaneObject("FLOOR_PLANE", new Vector3d(0f, 1f, 0f), new Vector3d(0f, 1f, 0f), checkerMaterial));
            //raytracer.Scene.Objects.Add(new PlaneObject("VERTICAL_PLANE", new Vector3d(15f, 0f, 0f), new Vector3d(-1f, 0f, 0f), checkerMaterial));

            PointLight cameraLight = new PointLight(new Color3f(246.0/255.0, 205.0/255.0, 139.0/255.0), raytracer.Camera.Position);
            cameraLight.DiffusePower = 6f;
            //cameraLight.SpecularPower = 10f;
            raytracer.Scene.Lights.Add(cameraLight);

            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(10d, 4d, 3d)));
            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(10d, 7d, 0d)));
            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(10d, 13,-3d)));

            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(9d, 1, -3d)));

            //raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(8d, 2d, 3d)));
            //raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(10d, 2d, 1d)));

            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(8d, 4d, 1d)));
            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(12d, 4d, 5d)));
            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(12d, -2d, 5d)));

            //raytracer.Camera.Position = new Vector3d(14d, -6d, 7d);
            //raytracer.Camera.Direction = new Vector3d(1d, 0d, 0d);
            //raytracer.Camera.LookAt(new Vector3d(10d, 2d, 3d));

            raytracer.RenderProgress = new Rendering.Raytracer.ProgressDelegate(RaytracerProgressUpdate);
            raytracer.RenderComplete = new Rendering.Raytracer.RenderCompleteDelegate(RaytracerRenderComplete);

            Thread thread = new Thread(raytracer.Render);
            thread.Start();
        }

        private void RaytracerProgressUpdate(float progress)
        {
            float a = progress;

            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Raytracer.Rendering.Raytracer.ProgressDelegate(RaytracerProgressUpdate), progress);
            }
            else
            {
                progressBar1.Value = (int)(progress * 100);
            }
        }

        private void RaytracerRenderComplete(RenderTarget result)
        {
            _raytracerResult = result;
            _raytracerOutput = result.ColorBuffer.Bitmap; //result.DepthBuffer.ToBitmap();//
            _raytracerOutput.Save(string.Format(@"..\..\Data\Results\{0}.bmp", Environment.TickCount));

            //AntiAliasingRenderer antiAliasingRenderer = new AntiAliasingRenderer(_raytracerOutput);
            //_raytracerOutput = antiAliasingRenderer.Render(2);

            //_raytracerOutput.Save(string.Format(@".\{0}.bmp", Environment.TickCount));

            RaytracerProgressUpdate(0.0f);

            if (InvokeRequired)
            {
                this.Invoke(new Action(Refresh));
            }

            //DepthBlurRenderer depthBlurRenderer = new DepthBlurRenderer(result, 1.7, 1.9, 17);
            DepthBlurRenderer depthBlurRenderer = new DepthBlurRenderer(result, 1.4, 1.6, 9);
            _raytracerOutput = depthBlurRenderer.Render();
            _raytracerOutput.Save(string.Format(@"..\..\Data\Results\{0}.bmp", Environment.TickCount));

            if (InvokeRequired)
            {
                this.Invoke(new Action(Refresh));
            }

            /*
            Bitmap blurResult = result.ColorBuffer.Bitmap;

            _animation.ImageStack.Bitmaps.Add(blurResult);

            for (int i = 0; i < 200; i++)
            {
                BoxBlurRenderer boxBlurRenderer = new BoxBlurRenderer(blurResult, 5);
                blurResult = boxBlurRenderer.Render();

                _animation.ImageStack.Bitmaps.Add(blurResult);
            }

            _animation.FrameUpdate += new Animation.FrameUpdateDelegate(AnimationFrameUpdate);
            _animation.Start(10);
            */
        }

        private void AnimationFrameUpdate(Bitmap frame)
        {
            _raytracerOutput = frame;
            Invalidate();
        }

        private void Form1MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PixelateRenderer pixelate = new PixelateRenderer(_raytracerOutput, new Rectangle(System.Math.Max(0, e.X - 25), System.Math.Max(0, e.Y - 25), 50, 50), new Size(5, 5));
                _raytracerOutput = pixelate.Render();

                Invalidate();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                string data;

                if (_raytracerResult != null)
                {
                    Color color = _raytracerResult.ColorBuffer.Bitmap.GetPixel(e.X, e.Y);
                    data = string.Format("Color: {0}, {1}, {2}\r\nDepth: {3}\r\n({4}, {5})", color.R, color.G, color.B, _raytracerResult.DepthBuffer[e.Y, e.X], e.X, e.Y);
                }
                else
                {
                    data = string.Format("({0}, {1})", e.X, e.Y);
                }
                MessageBox.Show(data);
            }
            else
            {
                BoxBlurRenderer boxBlurRenderer = new BoxBlurRenderer(_raytracerOutput, 5);
                _raytracerOutput = boxBlurRenderer.Render();

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_raytracerOutput != null)
            {
                e.Graphics.DrawImage(_raytracerOutput, 0, 0);
            }
        }
    }
}