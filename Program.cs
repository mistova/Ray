﻿using System;
using System.Numerics;

namespace Ray{
    class Program{
        class Material
        {
            public Vector3 color;
            public Material()
            {
                this.color = new Vector3(0, 0, 0);
            }
            public Material(Vector3 clr)
            {
                color = clr;
            }
        }
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
        class Sphere{
            public Vector3 center;
            public float radius;
            public Material material;
            public Sphere(){
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

            public bool RayIntersect(Vector3 orig, Vector3 dir, ref float t0){
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
            float diffuseLightIntensity = 0;
            for (int i = 0; i < lights.Length; i++)
            {
                Vector3 lightDir = Vector3.Normalize(lights[i].position - point);
                diffuseLightIntensity += lights[i].intensity * Math.Max(0, Vector3.Dot(lightDir, N));
            }
            return material.color * diffuseLightIntensity;
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
        static void Main(string[] args){
            Console.WriteLine("Start");
            Material ivory = new Material(new Vector3(0.4f, 0.4f, 0.3f));
            Material redRubber = new Material(new Vector3(0.3f, 0.1f, 0.1f));

            Sphere[] spheres = new Sphere[4];
            spheres[0] = new Sphere(new Vector3(-3, 0, -16), 2, ivory);
            spheres[1] = new Sphere(new Vector3(-1.0f, -1.5f, -12), 2, redRubber);
            spheres[2] = new Sphere(new Vector3(1.5f, -0.5f, -18), 3, redRubber);
            spheres[3] = new Sphere(new Vector3(7, 5, -18), 4, ivory);

            Light[] lights = new Light[1];
            lights[0] = new Light(new Vector3(-20, 20, 20), 1.5f);

            Render(spheres, lights);
            Console.WriteLine("End");
        }
    }
}