using System.Numerics;

namespace Ray
{
    public class Material
    {
        Vector3 color;
        Vector2 albedo;

        public Material()
        {
            this.color = new Vector3(0, 0, 0);
            this.albedo = new Vector2(1, 0);
            this.SpecularExponent = 0;
        }
        public Material(Vector3 clr, Vector2 alb, float spec)
        {
            this.color = clr;
            this.albedo = alb;
            this.SpecularExponent = spec;
        }

        public Vector3 Color { get => color; set => color = value; }
        public Vector2 Albedo { get => albedo; set => albedo = value; }
        public float SpecularExponent { get; set; }
    }
}
