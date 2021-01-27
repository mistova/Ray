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
            
            fileStream.Write(headerBytes);
            foreach (var cell in framebuffer) {
                    fileStream.Write(Convert.ToByte((int)(255 * cell.X)));
                    fileStream.Write(Convert.ToByte((int)(255 * cell.Y)));
                    fileStream.Write(Convert.ToByte((int)(255 * cell.Z)));
            }
            fileStream.Close();
        }
    }
}
