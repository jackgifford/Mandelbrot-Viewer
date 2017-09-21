using MandelbrotViewer.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotViewer.Services
{
    public class SetBitmap
    {
        private int TILE_SIZE = 512;

        public Bitmap ManipulateBitmap(int[][] stepGrid)
        {

            var bmp = new Bitmap(TILE_SIZE, TILE_SIZE);
            var bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int pixelSize = 4;

            unsafe
            {
                for (int y = 0; y < bmd.Height; y++)
                {
                    byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                    for (int x = 0; x < bmd.Width; x++)
                    {
                        var color = GetColour(stepGrid[y][x]);

                        row[x * pixelSize] = color.blue;
                        row[x * pixelSize + 1] = color.green;
                        row[x * pixelSize + 2] = color.red;
                        row[x * pixelSize + 3] = 255;
                    }
                }
            }

            bmp.UnlockBits(bmd);

            return bmp;
        }

        private Pixel GetColour(int steps)
        {
            var color = new Pixel();


            if (steps < 42)
            {
                color.red = 255;
                color.green = 129;
                color.blue = 99;
            }
            else if (steps < 85)
            {
                color.red = 234;
                color.green = 182;
                color.blue = 214;
            }
            else if (steps < 128)
            {
                color.red = 181;
                color.green = 112;
                color.blue = 255;
            }
            else if (steps < 173)
            {
                color.red = 90;
                color.green = 121;
                color.blue = 232;
            }
            else
            {
                color.red = 169;
                color.green = 247;
                color.blue = 255;
            }
            return color;
        }

    }
}
