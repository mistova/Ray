using System;
using System.Numerics;

namespace Ray{
    class Program{
        static bool SceneIntersect(Vector3 orig, Vector3 dir, Sphere[] spheres, ref Vector3 hit, ref Vector3 N, ref Material material) {
            float spheres_dist = float.MaxValue;
            for (int i=0; i<spheres.Length; i++) {
                float dist_i = 0;
                if (spheres[i].RayIntersect(orig, dir, ref dist_i) && dist_i<spheres_dist) {
                    spheres_dist = dist_i;
                    hit = orig + dir * dist_i;
                    N = Vector3.Normalize((hit - spheres[i].center));
                    material = spheres[i].material;
                }
            }
            return spheres_dist<1000;
        }
        static Vector3 CastRay(Vector3 orig, Vector3 dir, Sphere[] spheres, Light[] lights){
            Vector3 point = new Vector3(), N = new Vector3();
            Material material = new Material();
            if (!SceneIntersect(orig, dir, spheres, ref point, ref N, ref material))
                return new Vector3(1, 1, 1);
            float diffuseLightIntensity = 0, specularLightIntensity = 0;
            for (int i = 0; i < lights.Length; i++)
            {
                Vector3 lightDir = Vector3.Normalize(lights[i].position - point);

                float lightDistance = (lights[i].position - point).Length();

                Vector3 shadowOrig = Vector3.Dot(lightDir, N)  < 0 ? point - N * (float) 1e-3 : point + N * (float) 1e-3;
                Vector3 shadowPt = new Vector3(), shadowN = new Vector3();
                Material tmpMaterial = new Material();
                if (SceneIntersect(shadowOrig, lightDir, spheres, ref shadowPt, ref shadowN, ref tmpMaterial) && (shadowPt - shadowOrig).Length() < lightDistance)
                    continue;
                diffuseLightIntensity += lights[i].intensity * Math.Max(0, Vector3.Dot(lightDir, N));
                specularLightIntensity += (float) Math.Pow(Math.Max(0, - Vector3.Dot(Reflect(lightDir, N), dir)), material.SpecularExponent) * lights[i].intensity;
            }
            return (material.Color * diffuseLightIntensity * material.Albedo.X) + (new Vector3(1, 1, 1) * specularLightIntensity * material.Albedo.Y);
        }
        static Vector3 Reflect(Vector3 I, Vector3 N){
            return I - N * 2.0f * Vector3.Dot(I, N);
        }
    static void Render(Sphere[] spheres, Light[] lights){
            const int width = 1024;
            const int height = 768;
            Vector3[] framebuffer = new Vector3[width * height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    float x = (float)((2 * (i + 0.5) / (float)width - 1) * Math.Tan(Math.PI / 4f) * width / (float)height);
                    float y = (float)(-(2 * (j + 0.5) / (float)height - 1) * Math.Tan(Math.PI / 4f));
                    Vector3 dir = Vector3.Normalize(new Vector3(x, y, -1f));
                    framebuffer[i + j * width] = CastRay(new Vector3(0, 0, 0), dir, spheres, lights);
                }
            }
            string path = "C:/Users/windo/Desktop/out.ppm";
            FileWriter.PPMWriter(framebuffer, width, height, path);
        }
        static void Main(){
            Console.WriteLine("Start");

            Material ivory = new Material(new Vector3(0.4f, 0.4f, 0.3f), new Vector2(0.6f, 0.3f), 50);
            Material redRubber = new Material(new Vector3(0.3f, 0.1f, 0.1f), new Vector2(0.9f, 0.1f), 10);

            Sphere[] spheres = new Sphere[4];
            spheres[0] = new Sphere(new Vector3(-3, 0, -16), 2, ivory);
            spheres[1] = new Sphere(new Vector3(-1.0f, -1.5f, -12), 2, redRubber);
            spheres[2] = new Sphere(new Vector3(1.5f, -0.5f, -18), 3, redRubber);
            spheres[3] = new Sphere(new Vector3(0, 0, -60), 30, ivory);

            Light[] lights = new Light[4];
            lights[0] = new Light(new Vector3(-20, 20, 20), 1.5f);
            lights[0] = new Light(new Vector3(-20, 20, 20), 1.5f);
            lights[1] = new Light(new Vector3(-20, 20, 20), 1.5f);
            lights[2] = new Light(new Vector3(30, 50, -25), 1.8f);
            lights[3] = new Light(new Vector3(30, 20, 30), 1.7f);

            Render(spheres, lights);
            Console.WriteLine("End");
        }
    }
}
