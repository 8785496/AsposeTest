using Aspose.Imaging;
using Aspose.Imaging.FileFormats.Tiff.Enums;
using Aspose.Imaging.ImageFilters.FilterOptions;
using Aspose.Imaging.ImageOptions;
using AsposeTest.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeTest.Services
{
    public class ImageService : IImageService
    {
        public FileStream GaussianBlurFilter(string fileName, string fileType, int radius, double sigma)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
            using (var image = Image.Load(filePath))
            {
                RasterImage rasterImage = (RasterImage)image;
                // Apply a Gaussian blur filter
                rasterImage.Filter(rasterImage.Bounds, new GaussianBlurFilterOptions(radius, sigma));

                switch (fileType)
                {
                    case "jpeg":
                        rasterImage.Save(filePath + "_GaussianBlurFilter.jpg", new JpegOptions());
                        return File.OpenRead(filePath + "_GaussianBlurFilter.jpg");
                    case "png":
                        rasterImage.Save(filePath + "_GaussianBlurFilter.png", new PngOptions());
                        return File.OpenRead(filePath + "_GaussianBlurFilter.png");
                    case "bmp":
                        rasterImage.Save(filePath + "_GaussianBlurFilter.bmp", new BmpOptions());
                        return File.OpenRead(filePath + "_GaussianBlurFilter.bmp");
                    case "tiff":
                        rasterImage.Save(filePath + "_GaussianBlurFilter.tiff", new TiffOptions(TiffExpectedFormat.TiffLzwRgb));
                        return File.OpenRead(filePath + "_GaussianBlurFilter.tiff");
                    default:
                        throw new Exception();
                }
            }
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            if (file.Length > 0)
            {
                var rootFolder = Directory.GetCurrentDirectory();
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(rootFolder, "Uploads", fileName);
                using (var stream = File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                return fileName;
            }
            throw new Exception("File not saved");
        }

        public FileStream GetPreview(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
            using (var image = Image.Load(filePath))
            {
                image.Save(filePath + "_preview.jpg", new JpegOptions());
            }
            return File.OpenRead(filePath + "_preview.jpg");
        }
    }
}
