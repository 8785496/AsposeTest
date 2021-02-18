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

        /// <summary>
        /// This method saves image file on the server and returns the filename
        /// </summary>
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

        /// <summary>
        /// This method returns an image preview file in JPG format
        /// </summary>
        public IActionResult Preview(string fileName)
        {
            var fileStream = _imageService.GetPreview(fileName);
            return File(fileStream, "image/jpeg");
        }

        /// <summary>
        /// This method returns a processed image with a blur effect
        /// </summary>
        public IActionResult GaussianBlur(string fileType, string fileName, int radius = 5, double sigma = 4.0)
        {
            var fileStream = _imageService.GaussianBlurFilter(fileName, fileType, radius, sigma);
            return File(fileStream, $"image/{fileType}");
        }
    }
}
