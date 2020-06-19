using System.Collections.Generic;

using Raytracer.Math;
using Raytracer.Objects;
using Raytracer.Lights;
using Raytracer.Rendering.Intersection;

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

        public IIntersectionResult GetNearestObjectIntersection(Vector3d direction, Vector3d position, AbstractObject3d excludeObject = null)
        {
            IIntersectionResult nearestIntersectionResult = null;

            foreach (AbstractObject3d o in Objects)
            {
                if (excludeObject == o)
                {
                    continue;
                }

                IIntersectionResult intersectionResult = o.Intersection(direction, position);

                if (intersectionResult != null)
                {
                    if (nearestIntersectionResult == null)
                    {
                        nearestIntersectionResult = intersectionResult;
                    }
                    else if (intersectionResult.IntersectionDistance < nearestIntersectionResult.IntersectionDistance)
                    {
                        nearestIntersectionResult = intersectionResult;
                    }
                }

                /*
                // TODO move this to o.Intersection (distance, setting of object, return intersectionresult)
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
                */
            }

            return nearestIntersectionResult;
        }
    }
}
