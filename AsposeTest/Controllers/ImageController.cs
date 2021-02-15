using Aspose.Imaging;
using Aspose.Imaging.FileFormats.Tiff.Enums;
using Aspose.Imaging.ImageFilters.FilterOptions;
using Aspose.Imaging.ImageOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeTest.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file.Length > 0)
            {
                var rootFolder = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(rootFolder, "Uploads", file.FileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { name = file.FileName });
        }

        public IActionResult Preview(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
            using (var image = Image.Load(filePath))
            {
                image.Save(filePath + "_preview.jpg", new JpegOptions());
            }
            var imageFileStream = System.IO.File.OpenRead(filePath + "_preview.jpg");
            return File(imageFileStream, "image/jpeg");
        }

        public IActionResult GaussianBlur(string fileType, string fileName, int radius = 5, double sigma = 10.0)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
            using (var image = Image.Load(filePath))
            {
                RasterImage rasterImage = (RasterImage)image;
                // Apply a Gaussian blur filter

                switch (fileType)
                {
                    case "jpg":
                        rasterImage.Filter(rasterImage.Bounds, new GaussianBlurFilterOptions(radius, sigma));
                        rasterImage.Save(filePath + "_GaussianBlurFilter.jpg", new JpegOptions());
                        break;
                    case "png":
                        rasterImage.Filter(rasterImage.Bounds, new GaussianBlurFilterOptions(radius, sigma));
                        rasterImage.Save(filePath + "_GaussianBlurFilter.png", new PngOptions());
                        break;
                    case "bmp":
                        rasterImage.Filter(rasterImage.Bounds, new GaussianBlurFilterOptions(radius, sigma));
                        rasterImage.Save(filePath + "_GaussianBlurFilter.bmp", new BmpOptions());
                        break;
                    case "tiff":
                        rasterImage.Filter(rasterImage.Bounds, new GaussianBlurFilterOptions(radius, sigma));
                        rasterImage.Save(filePath + "_GaussianBlurFilter.tiff", new TiffOptions(TiffExpectedFormat.Default));
                        break;
                    default:
                        break;
                }

                
            }

            FileStream imageFileStream;
            switch (fileType)
            {
                case "jpg":
                    imageFileStream = System.IO.File.OpenRead(filePath + "_GaussianBlurFilter.jpg");
                    return File(imageFileStream, "image/jpeg");
                case "png":
                    imageFileStream = System.IO.File.OpenRead(filePath + "_GaussianBlurFilter.png");
                    return File(imageFileStream, "image/png");
                case "bmp":
                    imageFileStream = System.IO.File.OpenRead(filePath + "_GaussianBlurFilter.bmp");
                    return File(imageFileStream, "image/bmp");
                case "tiff":
                    imageFileStream = System.IO.File.OpenRead(filePath + "_GaussianBlurFilter.tiff");
                    return File(imageFileStream, "image/tiff");
                default:
                    break;
            }

            return Ok();
        }
    }
}
