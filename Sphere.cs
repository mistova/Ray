using System;
using System.Numerics;

namespace Ray
{
    class Sphere
    {
        public Vector3 center;
        public float radius;
        public Material material;
        public Sphere()
        {
            center = new Vector3(5, 5, 5);
            radius = 2f;
            material = new Material();
        }
        public Sphere(Vector3 c, float r, Material m)
        {
            center = c;
            radius = r;
            material = m;
        }

        public bool RayIntersect(Vector3 orig, Vector3 dir, ref float t0)
        {
            Vector3 L = center - orig;
            float tca = Vector3.Dot(L, dir);
            float d2 = Vector3.Dot(L, L) - (tca * tca);
            if (d2 > radius * radius)
                return false;
            float thc = (float)Math.Sqrt(radius * radius - d2);
            t0 = tca - thc;
            float t1 = tca + thc;
            if (t0 < 0)
                t0 = t1;
            if (t0 < 0)
                return false;
            return true;
        }
    }
}
