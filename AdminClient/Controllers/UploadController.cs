using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AdminClient.AppHelper.PublitioApi;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AdminClient.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<UploadController> _logger;

        public UploadController(IWebHostEnvironment hostingEnvironment,
                              ILogger<UploadController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        [HttpPost]
        public async Task<string> ImgUpload(IFormFile file)
        {
            PublitioApi publitioApi = new PublitioApi("Vj1qW6Qpaxrsemv1wbtu", "z2Vq4MQHNGXdVqXxiBfJSaL5XgSAg1NT");
            try
            {
                using (var ImgFile = System.IO.File.OpenRead(file.FileName))
                {
                    var res1 = await publitioApi.UploadFileAsync("/images/pp/", new Dictionary<string, object> { ["title"] = "XX" }, ImgFile);
                    //Console.WriteLine(res1);
                }
                // List files
                var res = await publitioApi.GetAsync("/images/pp/", new Dictionary<string, object> { ["limit"] = 3 });

                //string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                //filename = EnsureCorrectFilename(filename);

                // Get the ID of the first file
                var files = (JArray)res["files"];
                var firstFile = (JObject)files[0];
                var id = (string)firstFile["id"];
                // Console.WriteLine(id);

                //string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/pp");
                //string uniqueFileName = Guid.NewGuid().ToString() + "_" + filename;
                //string imagePath = Path.Combine(uploadsFolder, uniqueFileName);
                //file.CopyTo(new FileStream(imagePath, FileMode.Create));
                return "/images/pp/" + id;
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
