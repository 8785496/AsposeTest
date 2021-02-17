using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeTest.Interfaces
{
    public interface IImageService
    {
        FileStream GaussianBlurFilter(string fileName, string fileType, int radius, double sigma);
        Task<string> SaveFile(IFormFile file);
        FileStream GetPreview(string fileName);
    }
}
