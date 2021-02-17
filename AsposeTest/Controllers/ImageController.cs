using AsposeTest.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AsposeTest.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var reg = new Regex(@"image\/(png|jpg|jpeg|bmp|tiff)", RegexOptions.IgnoreCase);
            if (!reg.Match(file.ContentType).Success)
            {
                return BadRequest(new { error = "Invalid file format" });
            }
            try
            {
                var fileName = await _imageService.SaveFile(file);
                return Ok(new { name = fileName });
            }
            catch (Exception)
            {
                return BadRequest(new { error = "File not uploaded" });
            }
        }

        public IActionResult Preview(string fileName)
        {
            var fileStream = _imageService.GetPreview(fileName);
            return File(fileStream, "image/jpeg");
        }

        public IActionResult GaussianBlur(string fileType, string fileName, int radius = 5, double sigma = 10.0)
        {
            var fileStream = _imageService.GaussianBlurFilter(fileName, fileType, radius, sigma);
            return File(fileStream, $"image/{fileType}");
        }
    }
}
