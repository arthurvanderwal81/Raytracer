using System.Collections.Generic;

using Raytracer.Math;
using Raytracer.Objects;
using Raytracer.Lights;

namespace Raytracer.Rendering
{
    public class Scene
    {
        public List<AbstractObject3d> Objects { get; private set; }
        public List<AbstractLight> Lights { get; private set; }

        public Scene()
        {
            Objects = new List<AbstractObject3d>();
            Lights = new List<AbstractLight>();
        }

        public IntersectionResult GetNearestObjectIntersection(Vector3d direction, Vector3d position, AbstractObject3d excludeObject = null)
        {
            IntersectionResult intersectionResult = new IntersectionResult();

            foreach (AbstractObject3d o in Objects)
            {
                if (excludeObject == o)
                {
                    continue;
                }

                Vector3d intersection = o.Intersection(direction, position);

                if (intersection != null)
                {
                    double intersectionDistance = (intersection - position).Length();

                    if (intersectionDistance < intersectionResult.IntersectionDistance)
                    {
                        intersectionResult.IntersectionDistance = intersectionDistance;
                        intersectionResult.Intersection = intersection;
                        intersectionResult.Object = o;
                    }
                }
            }

            return intersectionResult;
        }
    }
}
