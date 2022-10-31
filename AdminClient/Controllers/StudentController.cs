using AdminClient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;

namespace AdminClient.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;
        private string appMenu = "AppMenu", userLogCode = "UserLogCode", tokenTxt = "TokenTxt",
           userId = "UserId", schoolId = "SchoolId", imagePath = "ImagePath", fullName = "FullName", roleName = "RoleName";

        public StudentController(IConfiguration configuration, ILogger<StudentController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        public async Task<IActionResult> Index()
        {
            string returnString = string.Empty;
            string token = HttpContext.Session.GetString(tokenTxt);
            string url = _apiBaseUrl + "/api/Users/GetStudentList";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    returnString = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(returnString.ToString().Replace("&#xD;&#xA;", Environment.NewLine));
                    ViewBag.Data = json;
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(string? id)
        {
            StudentModel studentModel = new StudentModel();
            string token = HttpContext.Session.GetString(tokenTxt);
            int studentId = Convert.ToInt32(id);
            if (studentId > 0)
            {
                string url = _apiBaseUrl + $"/api/Student/GetStudent/{studentId}";
                var edit_request = new HttpRequestMessage(HttpMethod.Get, url);
                edit_request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage edit_response = await client.SendAsync(edit_request);

                    if (edit_response.StatusCode == HttpStatusCode.OK)
                    {
                        string returnString = await edit_response.Content.ReadAsStringAsync();
                        studentModel = JsonConvert.DeserializeObject<StudentModel>(returnString);
                    }
                }
            }
            
            return View(studentModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveStudentData(StudentModel _studentModel)
        {
            string stringData = JsonConvert.SerializeObject(_studentModel);
            string token = HttpContext.Session.GetString(tokenTxt);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            string url = _apiBaseUrl + "/api/Student/CreateStudent/";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = contentData;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Student not created!Check please.");
                }
            }
            return RedirectToAction("Index");
        }

    }
}
