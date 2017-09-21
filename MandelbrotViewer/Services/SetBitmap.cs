using MandelbrotViewer.Service;
using System.Drawing;
using System.Drawing.Imaging;


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

            if (steps == - 1)
                steps = 0;

            if (steps < 85)
            {
                color.red = 0;

                double greenGradient = 200.0f / 85.0f;
                color.green = (byte)(steps * greenGradient);

                color.blue = 255;
            }

            else if (steps < 170)
            {
                double redGradient = 255.0f / 85.0f;
                color.red = (byte)((steps - 85) * redGradient);

                color.green = 200;

                double blueGradient = -255.0f / 85.0f;
                color.blue = (byte)(255 + ((steps - 85) * blueGradient));

            }

            else
            {
                color.red = 255;

                double greenGradient = -200.0f / 85.0f;
                color.green = (byte)(200 + ((steps - 170) * greenGradient));

                color.blue = 0;
            }

            return color;
        }

    }
}
