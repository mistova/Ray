using System.Numerics;

namespace Ray
{
    class Light
    {
        public Vector3 position;
        public float intensity;
        public Light()
        {

        }
        public Light(Vector3 p, float i)
        {
            position = p;
            intensity = i;
        }
    }
}
