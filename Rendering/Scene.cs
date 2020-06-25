using System.Linq;
using System.Collections.Generic;

using Raytracer.Math;
using Raytracer.Objects;
using Raytracer.Lights;
using Raytracer.Rendering.Intersection;

namespace Raytracer.Rendering
{
    public class Scene
    {
        private List<AbstractObject3d> _visibleObjects;

        public List<AbstractObject3d> Objects { get; set; }
        public List<AbstractLight> Lights { get; set; }

        public Scene()
        {
            Objects = new List<AbstractObject3d>();
            Lights = new List<AbstractLight>();
        }

        /*
        public void AddObject(AbstractObject3d object3d)
        {
            if (Objects.Any(o => o.Name == object3d.Name))
            {
                throw new Exception("Object name not unique");
            }

            Objects.Add(object3d);
        }

        public void AddLight(AbstractLight light)
        {
            Lights.Add(light);
        }
        */

        public void UpdateVisibleObjects(Camera camera)
        {
            _visibleObjects = new List<AbstractObject3d>();

            foreach (AbstractObject3d o in Objects)
            {
                if (o.UpdateVisibility(camera))
                {
                    _visibleObjects.Add(o);
                }
            }

            //_visibleObjects = Objects.Where(o => o.IsVisible(camera)).ToList();
        }

        public IIntersectionResult GetNearestObjectIntersection(Vector3d direction, Vector3d position, AbstractObject3d excludeObject = null)
        {
            IIntersectionResult nearestIntersectionResult = null;

            //foreach (AbstractObject3d o in Objects)
            foreach (AbstractObject3d o in _visibleObjects)
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
            }

            return nearestIntersectionResult;
        }
    }
}