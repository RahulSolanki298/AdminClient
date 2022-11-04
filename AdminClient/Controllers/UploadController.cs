using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminClient.AppHelper.PublitioApi;

namespace AdminClient.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<UploadController> _logger;
        PublitioApi publitioApi = new PublitioApi("Vj1qW6Qpaxrsemv1wbtu", "z2Vq4MQHNGXdVqXxiBfJSaL5XgSAg1NT");
        public UploadController(IWebHostEnvironment hostingEnvironment,
                              ILogger<UploadController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        [HttpPost]
        public async Task<string> ImgUpload(IFormFile file)
        {
            try
            {
                string filepath = String.Empty;
                using (var readfile = file.OpenReadStream())// System.IO.File.OpenRead(file))
                {
                    var res1 = await publitioApi.UploadFileAsync(
                          "files/create",
                          new Dictionary<string, object> { ["title"] = "XX" },
                          readfile);
                    filepath = (string)res1.Root["url_preview"];
                    Console.WriteLine(res1);
                }

                string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                //FileStream(file,FileMode.Open);
                filename = EnsureCorrectFilename(filename);

                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/pp");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + filename;
                string imagePath = Path.Combine(uploadsFolder, uniqueFileName);
                file.CopyTo(new FileStream(imagePath, FileMode.Create));
                return filepath;// "/images/pp/" + uniqueFileName;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return "";
            }
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }
    }
}
