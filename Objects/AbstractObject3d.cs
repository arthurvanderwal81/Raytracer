using System.Drawing;

using Raytracer.Math;
using Raytracer.Lights;
using Raytracer.Materials;
using Raytracer.Rendering;

namespace Raytracer.Objects
{
    public abstract class AbstractObject3d
    {
        public string Name { get; set; }
        public Material Material { get; set; }

        protected abstract Vector3d GetNormal(Vector3d positionOnObject);

        protected abstract Vector2d GetUVCoordinates(Vector3d positionOnObject);

        public abstract Vector3d Intersection(Vector3d direction, Vector3d position);

        public virtual Color GetColor(Vector3d direction, Vector3d position, Scene scene)
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
                double specularIntensity = System.Math.Pow(ScalarHelpers.Saturate(normalDotHalfwayVector), 16f);

                Color3f specular = specularIntensity * light.SpecularColor * light.SpecularPower / distanceSquared;

                //return (surfaceColor * (diffuse + specular)).ToColor();
                result += diffuse + specular;
            }

            return (((surfaceColor * result) * (1.0 - Material.Reflection)) + (reflectionColor * Material.Reflection)).ToColor();
        }
    }
}