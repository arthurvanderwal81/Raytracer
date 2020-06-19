using Raytracer.Math;
using Raytracer.Materials;

namespace Raytracer.Objects
{
    public class SphereObject : AbstractObject3d
    {
        private Vector3d _position;
        private double _radius;

        public SphereObject(string name, Vector3d position, double radius, Material material)
        {
            _position = position;
            _radius = radius;

            Name = name;
            Material = material;
        }

        private Vector3d ProjectSpherePosition(Vector3d direction, Vector3d position)
        {
            double directionFactor = (_position - position).Dot(direction);

            if (directionFactor < 0.0f)
            {
                return null;
            }

            Vector3d spherePositionProjected = directionFactor * direction;

            return spherePositionProjected;
        }

        protected override Vector3d GetNormal(Vector3d positionOnSphere)
        {
            Vector3d normal = positionOnSphere - _position;

            if (Material.InvertNormals)
            {
                normal = -normal;
            }

            return normal.Normalize();
        }

        public override Vector3d Intersection(Vector3d direction, Vector3d position)
        {
            Vector3d spherePositionProjected = ProjectSpherePosition(direction, position);

            if (spherePositionProjected == null)
            {
                return null;
            }

            Vector3d distance = (_position - position) - spherePositionProjected;

            if (distance.Length() > _radius)
            {
                return null;
            }

            double c = System.Math.Sqrt(_radius * _radius - distance.Length() * distance.Length());

            double i1 = ((_position - position).Dot(direction) - c);
            double i2 = ((_position - position).Dot(direction) + c);

            return position + direction * i1;
        }

        protected override Vector2d GetUVCoordinates(Vector3d position)
        {
            Vector3d normal = GetNormal(position);

            double u = (double)(System.Math.Atan2(normal.X, normal.Z) / (2.0f * System.Math.PI) + 0.5f);
            double v = -normal.Y * 0.5f + 0.5f;

            return new Vector2d(u, v);
        }

        /*
        public override Color GetColor(Vector3d direction, Vector3d position, Scene scene)
        {
            Color3f result = new Color3f();

            Color3f surfaceColor = Material.Texture != null ? Material.Texture.GetColor(GetUVCoordinates(position), 0) : Material.Color;
            Color3f reflectionColor = new Color3f();

            if (Material.Reflection > 0.0f)
            {
                // Reflection
                Vector3d reflectionVector = VectorHelpers.GetReflectionVector(direction, GetNormal(position));

                IntersectionResult intersectionResult = scene.GetNearestObjectIntersection(reflectionVector, position, this);

                if (intersectionResult.Object != null)
                {
                    reflectionColor = new Color3f(intersectionResult.Object.GetColor(direction, intersectionResult.Intersection, scene));
                }
            }

            foreach (AbstractLight light in scene.Lights)
            {
                bool lightVisible = true;

                if (light.Position != null)
                {
                    Vector3d lightDistanceVector = light.Position - position;
                    double lightDistance = lightDistanceVector.Length();

                    foreach (AbstractObject3d o in scene.Objects)
                    {
                        if (o == this)
                        {
                            continue;
                        }

                        if (o is AbstractLight)
                        {
                            continue;
                        }

                        Vector3d intersection = o.Intersection(Vector3d.Normalize(lightDistanceVector), position);

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

                if (!lightVisible)
                {
                    continue;
                }

                Vector3d normal = GetNormal(position);
                Vector3d lightDirection = -light.GetDirection(position);

                double distanceSquared = light.GetDistance(position);
                distanceSquared *= distanceSquared;

                double normalDotLightDirection = normal.Dot(lightDirection);
                double diffuseIntensity = ScalarHelpers.Saturate(normalDotLightDirection);

                Color3f diffuse = diffuseIntensity * light.DiffuseColor * light.DiffusePower / distanceSquared;

                Vector3d halfwayVector = (lightDirection - direction).Normalize();

                double normalDotHalfwayVector = normal.Dot(halfwayVector);
                double specularIntensity = (double)System.Math.Pow(ScalarHelpers.Saturate(normalDotHalfwayVector), 16f);

                Color3f specular = specularIntensity * light.SpecularColor * light.SpecularPower / distanceSquared;

                //return (surfaceColor * (diffuse + specular)).ToColor();
                result += diffuse + specular;
            }

            return (((surfaceColor * result) * (1.0 - Material.Reflection)) + (reflectionColor * Material.Reflection)).ToColor();
        }*/

        /*
        public Color GetColor(Vector3d direction, Vector3d position, Scene scene)
        {
            Color surfaceColor = Material.Texture != null ? Material.Texture.GetColor(GetUVCoordinates(position), 0) : Material.Color;
            Color reflectionColor = Color.Black;

            if (Material.Reflection > 0.0f)
            {
                // Reflection
                Vector3d reflectionVector = VectorHelpers.GetReflectionVector(direction, GetNormal(position));

                IntersectionResult intersectionResult = scene.GetNearestObjectIntersection(reflectionVector, position, this);

                if (intersectionResult.Object != null)
                {
                    return intersectionResult.Object.GetColor(direction, intersectionResult.Intersection, scene);
                }

                return Color.Black;
            }
            //else
            //{
            //    Color color = Material.Texture != null ? Material.Texture.GetColor(GetUVCoordinates(position), 0) : Material.Color;

            //    if (Material.FullBright)
            //    {
            //        return color;
            //    }

            //    // Light
            //    double lightIntensity = 0.1f;

            //    foreach (AbstractLight light in scene.Lights)
            //    {
            //        lightIntensity += light.GetIntensity(position, GetNormal(position));
            //    }

            //    if (lightIntensity > 1.0f)
            //    {
            //        lightIntensity = 1.0f;
            //    }

            //    return Color.FromArgb((int)(color.R * lightIntensity), (int)(color.G * lightIntensity), (int)(color.B * lightIntensity));
            //}

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
                    lightIntensity += light.GetIntensity(position, GetNormal(position));
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
        }
        */
    }
}