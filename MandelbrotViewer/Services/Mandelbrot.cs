using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotViewer.Service
{
    public class Mandelbrot
    {
        private int MAX_STEPS = 255;
        private int TILE_SIZE = 512;

        public Bitmap DrawMandelbrot(Complex center, int z)
        {
            var bmp = new Bitmap(TILE_SIZE, TILE_SIZE);

            var grid = EscapeGrid(center, z);

            for (var y = 0; y < TILE_SIZE; y++)
            {
                for (var x = 0; x < TILE_SIZE; x++)
                {
                    var pixel = GetColour(grid[y][x]);
                    var colour = Color.FromArgb(pixel.red, pixel.green, pixel.blue);
                    bmp.SetPixel(x, y, colour);

                }
            }

            return bmp;
        }


        private int EscapeSteps(Complex c)
        {
            int steps = 0;

            var z = new Complex
            {
                re = 0,
                im = 0
            };

            while (steps < MAX_STEPS && ((z.re * z.re) + (z.im * z.im) <= 4))
            {
                double newRe = ((z.re * z.re) - (z.im * z.im)) + c.re;

                z.im = 2 * z.im * z.re + c.im;
                z.re = newRe;

                steps++;
            }

            return steps;
        }

        // Fill a grid of TILE_SIZE by TILE_SIZE pixels, with the number of
        // steps each pixel took to escape the Mandelbrot set.
        private int[][] EscapeGrid(Complex center, int z)
        {
            var grid = new int[TILE_SIZE][];


            for (var y = 0; y < 512; y++)
            {
                grid[y] = new int[TILE_SIZE];
                for (var x = 0; x < 512; x++)
                {
                    var c = GenerateComplexSeed(z, x, y, center);
                    grid[y][x] = EscapeSteps(c);
                }
            }

            return grid;
        }

        private Complex GenerateComplexSeed(int z, int x, int y, Complex center)
        {
            double centerX = 512 / 2;
            double centerY = 512 / 2;

            double xDist = x - centerX;
            double yDist = y - centerY;

            Complex seed = new Complex
            {
                re = center.re + (xDist * Power(2, -z)),
                im = center.im + (yDist * Power(2, -z))
            };

            return seed;
        }

        //NO ONE MAN SHOULD HAVE ALL THAT POWER
        private double Power(double x, double n)
        {
            double result = 1;

            bool isNegative = false;
            if (n < 0)
            {
                n *= -1; //Makes n positive
                isNegative = true;
            }


            while (n > 0)
            {
                result *= x;
                n--;
            }

            if (isNegative)
            {
                result = 1 / result;
            }

            return result;
        }


        //Pretty sure this works but the bitmap isn't high resolution to verify...
        //Returns a greyscale colour based on the iteration count
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

    public class Pixel
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
    }


    public class Complex
    {
        public double im { get; set; }
        public double re { get; set; }

    }

}

