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

        [HttpGet]
        public IActionResult GetImage(string fileName)
        {
            var rootFolder = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(rootFolder, "Uploads", fileName);
            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg");
        }
    }
}
