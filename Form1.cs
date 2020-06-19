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

namespace Raytracer
{
    public partial class Form1 : Form
    {
        private Bitmap _raytracerOutput = null;

        public Form1()
        {
            InitializeComponent();

            MouseDown += Form1MouseDown;

            // TODO:
            // CHECK ALL CUBE FACE NORMALS
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

            this.ClientSize = new Size(1024, 768 + progressBar1.Height);

            //Raytracer raytracer = new Raytracer(this.ClientSize, false);
            Rendering.Raytracer raytracer = new Rendering.Raytracer(1024, 768, true);
            //Raytracer raytracer = new Raytracer(3840, 2160);
            //Raytracer raytracer = new Raytracer(7680, 4320);

            raytracer.Camera.Position = new Vector3d(5f, 5f, 2f);
            raytracer.Camera.LookAt(new Vector3d(10f, 2f, 0f));

            Vector3d focalPoint = raytracer.Camera.Position + raytracer.Camera.GetRayDirection(1024/2, 768/2) * raytracer.Camera.FocalLength;

            Material redMaterial = new Material();
            redMaterial.Color = new Color3f(Color.Red);

            Material greenMaterial = new Material();
            greenMaterial.Color = new Color3f(Color.Green);

            Material blueMaterial = new Material();
            blueMaterial.Color = new Color3f(Color.LightBlue);
            blueMaterial.Reflection = 0.1f;

            Material reflectiveMaterial = new Material();
            reflectiveMaterial.Color = new Color3f(.8, .8, .8);
            reflectiveMaterial.Reflection = .9f;

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

            raytracer.Scene.Objects.Add(new SphereObject("SPHERE_RIGHT",    new Vector3d(10f, 2f, 3f), 1f, woodMaterial));
            //raytracer.Scene.Objects.Add(new SphereObject("SPHERE_MIDDLE",   new Vector3d(10f, 2f, 0f), 1f, reflectiveMaterial));
            raytracer.Scene.Objects.Add(new CubeObject("CUBE_MIDDLE", new Vector3d(10f, 2f, 0f), 2f, testMaterials[5]));
            raytracer.Scene.Objects.Add(new SphereObject("SPHERE_LEFT",     new Vector3d(10f, 2f, -3f), 1f, blueMaterial));

            //raytracer.Scene.Objects.Add(new SphereObject("SPHERE_FOCUS", raytracer.Camera.Position + raytracer.Camera.Direction * raytracer.Camera.FocalLength, 0.3f, reflectiveMaterial));

            //raytracer.Scene.Objects.Add(new TriangleObject("VERTICAL_TRIANGLE_1", new Vertex(12f, 1f, -10f, 0f, 1f), new Vertex(12f, 1f, 10f, 1f, 1f), new Vertex(12f, 16f, 10f, 1f, 0f), backgroundMaterial1));
            //raytracer.Scene.Objects.Add(new TriangleObject("VERTICAL_TRIANGLE_2", new Vertex(12f, 16f, 10f, 1f, 0f), new Vertex(12f, 16f, -10f, 0f, 0f), new Vertex(12f, 1f, -10f, 0f, 1f), backgroundMaterial1));

            raytracer.Scene.Objects.Add(new QuadObject("VERTICAL_QUAD", new Vertex(12f, 1f, -10f, 0f, 1f), new Vertex(12f, 1f, 10f, 1f, 1f), new Vertex(12f, 16f, 10f, 1f, 0f), new Vertex(12f, 16f, -10f, 0f, 0f), backgroundMaterial1));

            //raytracer.Scene.Objects.Add(new TriangleObject("FLOOR_TRIANGLE_1", new Vertex(1f, 1f, -5f, 0f, 0f), new Vertex(1f, 1f, 5f, 2f, 0f), new Vertex(15f, 1f, 5f, 2f, 6f), checkerMaterial));
            //raytracer.Scene.Objects.Add(new TriangleObject("FLOOR_TRIANGLE_2", new Vertex(15f, 1f, 5f, 2f, 6f), new Vertex(15f, 1f, -5f, 0f, 6f), new Vertex(1f, 1f, -5f, 0f, 0f), checkerMaterial));

            raytracer.Scene.Objects.Add(new PlaneObject("FLOOR_PLANE", new Vector3d(0f, 1f, 0f), new Vector3d(0f, 1f, 0f), checkerMaterial));
            //raytracer.Scene.Objects.Add(new PlaneObject("VERTICAL_PLANE", new Vector3d(15f, 0f, 0f), new Vector3d(-1f, 0f, 0f), checkerMaterial));

            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), raytracer.Camera.Position));

            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(10d, 4d, 3d)));
            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(10d, 7d, 0d)));
            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(10d, 13,-3d)));

            raytracer.Scene.Lights.Add(new PointLight(new Color3f(1f, 1f, 1f), new Vector3d(9d, 1, -3d)));

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

        private void RaytracerRenderComplete(Bitmap result)
        {
            _raytracerOutput = result;
            _raytracerOutput.Save(string.Format(@"..\..\Data\Results\{0}.bmp", Environment.TickCount));

            //AntiAliasingRenderer antiAliasingRenderer = new AntiAliasingRenderer(_raytracerOutput);
            //_raytracerOutput = antiAliasingRenderer.Render(2);

            //_raytracerOutput.Save(string.Format(@".\{0}.bmp", Environment.TickCount));

            RaytracerProgressUpdate(0.0f);

            if (InvokeRequired)
            {
                this.Invoke(new Action(Refresh));
            }
        }

        private void Form1MouseDown(object sender, MouseEventArgs e)
        {
            MessageBox.Show(string.Format("{0}, {1}", e.X, e.Y));
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