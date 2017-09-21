using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MandelbrotViewer.Service;
using System.IO;
using System.Drawing.Imaging;

namespace MandelbrotViewer.Controllers
{
    public class ImageController : Controller
    {
        private readonly Mandelbrot _mandelbrot;

        public ImageController()
        {
            _mandelbrot = new Mandelbrot();
        }

        public IActionResult Index(int z, double x, double y)
        {
            var center = new Complex
            {
                re = x,
                im = -1 * y
            };

            var ms = new MemoryStream();

            var bmp = _mandelbrot.DrawMandelbrot(center, z);
            bmp.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            return new FileStreamResult(ms, "image/png");
        }
    }
}