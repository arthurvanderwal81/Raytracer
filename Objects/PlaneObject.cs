using Raytracer.Math;
using Raytracer.Materials;

namespace Raytracer.Objects
{
    public class PlaneObject : AbstractObject3d
    {
        private Vector3d _position;
        private Vector3d _normal;
        private Vector3d _uAxis;
        private Vector3d _vAxis;

        public PlaneObject(string name, Vector3d position, Vector3d normal, Material material)
        {
            _position = position;
            _normal = normal;

            Name = name;
            Material = material;

            GenerateUVAxes();
        }

        private void GenerateUVAxes()
        {
            Vector3d randomVector = new Vector3d(_normal.Y, _normal.X, _normal.Z);
            _vAxis = randomVector.Cross(_normal);
            _uAxis = _vAxis.Cross(_normal);

            _uAxis.Normalize();
            _vAxis.Normalize();                
        }

        public override Vector3d Intersection(Vector3d direction, Vector3d position)
        {
            double D = _position.Dot(_normal);
            double t = -(_normal.Dot(position) - D) / _normal.Dot(direction);

            if (t >= 0.0f)
            {
                return t * direction + position;
            }

            return null;
        }

        protected override Vector3d GetNormal(Vector3d positionOnObject)
        {
            return _normal;
        }

        protected override Vector2d GetUVCoordinates(Vector3d position)
        {
            Vector2d uvCoordinates = new Vector2d();

            uvCoordinates.X = _uAxis.Dot(position) % 1f;
            uvCoordinates.Y = _vAxis.Dot(position) % 1f;

            return uvCoordinates;
        }

        /*
        public Color GetColor(Vector3d direction, Vector3d position, Scene scene)
        {
            Color surfaceColor = Material.Texture != null ? Material.Texture.GetColor(GetUVCoordinates(position), 0) : Material.Color;
            Color reflectionColor = Color.Black;

            if (Material.Reflection > 0.0f)
            {
                // Reflection
                Vector3d reflectionVector = VectorHelpers.GetReflectionVector(direction, _normal);

                IntersectionResult intersectionResult = scene.GetNearestObjectIntersection(reflectionVector, position, this);

                if (intersectionResult.Object != null)
                {
                    reflectionColor = intersectionResult.Object.GetColor(direction, intersectionResult.Intersection, scene);
                }
            }

            double lightIntensity = 0.0f;

            foreach (AbstractLight light in scene.Lights)
            {
                bool lightVisible = true;

                if (light.Position != null)
                {
                    Vector3d lightDistanceVector = light.Position - position;
                    double lightDistance = lightDistanceVector.Length();
                    Vector3d lightDirection = Vector3d.Normalize(lightDistanceVector);

                    foreach (IObject3d o in scene.Objects)
                    {
                        if (o == this)
                        {
                            continue;
                        }

                        if (o is AbstractLight)
                        {
                            continue;
                        }

                        Vector3d intersection = o.Intersection(lightDirection, position);

                        if (intersection != null)
                        {
                            double intersectionDistance = (intersection - position).Length();

                            if (intersectionDistance < lightDistance)
                            {
                                lightVisible = false;
                            }
                        }
                    }
                }

                if (lightVisible)
                {
                    lightIntensity += light.GetIntensity(position, _normal);
                }
            }

            if (lightIntensity > 1.0f)
            {
                lightIntensity = 1.0f;
            }

            lightIntensity *= 1.0f - Material.Reflection;

            Color color = Color.FromArgb((int)(surfaceColor.R * lightIntensity + reflectionColor.R * Material.Reflection), 
                                         (int)(surfaceColor.G * lightIntensity + reflectionColor.G * Material.Reflection), 
                                         (int)(surfaceColor.B * lightIntensity + reflectionColor.B * Material.Reflection));

            return color;
        }*/
    }
}