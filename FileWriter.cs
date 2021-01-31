using System;
using System.IO;
using System.Numerics;

namespace Ray
{
    class FileWriter
    {
        public static void PPMWriter(Vector3[] framebuffer, int width, int height, string path) {
            FileStream stream = File.Create(path);
            BinaryWriter fileStream = new BinaryWriter(stream);

            string header = "P6\n" + width + " " + height + "\n255\n";
            byte[] headerBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(header);

            Vector3 c;

            fileStream.Write(headerBytes);
            foreach (var cell in framebuffer)
            {
                float max = Math.Max(cell.X, Math.Max(cell.Y, cell.Z));
                c = cell;
                if (max > 1)
                    c = cell / max;
                fileStream.Write(Convert.ToByte((int)(255 * c.X)));
                fileStream.Write(Convert.ToByte((int)(255 * c.Y)));
                fileStream.Write(Convert.ToByte((int)(255 * c.Z)));
            }
            fileStream.Close();
        }
    }
}
