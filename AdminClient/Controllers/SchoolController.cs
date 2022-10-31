
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AdminClient.ViewModels.Users;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog;
using AdminClient.ViewModels;

namespace AdminClient.Controllers
{

    public class SchoolController : Controller
    {
        private readonly ILogger<SchoolController> _logger;
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;
        private string appMenu = "AppMenu", userLogCode = "UserLogCode", tokenTxt = "TokenTxt",
           userId = "UserId", schoolId = "SchoolId", imagePath = "ImagePath", fullName = "FullName", roleName = "RoleName";

        public SchoolController(IConfiguration configuration, ILogger<SchoolController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }

        [HttpGet]
        public IActionResult TeachingPlan()
        {

            return View();
        }

        [HttpGet]
        public IActionResult AcademicCalendar()
        {
            return View();
        }


        [HttpGet]
        public IActionResult BookOrders()
        {
            return View();
        }

        //[HttpGet]
        public async Task<IActionResult> Index()
        {
            string returnString = string.Empty;
            string token = HttpContext.Session.GetString(tokenTxt);
            //var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            string url = _apiBaseUrl + "/api/Schools/GetSchools";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // request.Content = contentData;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    returnString = await response.Content.ReadAsStringAsync();
                    var menuList = JsonConvert.DeserializeObject(returnString);
                    JObject json = JObject.Parse(returnString.ToString().Replace("&#xD;&#xA;", Environment.NewLine));
                    ViewBag.Data = json;
                    //_logger.LogError("History not generated!Check please.");
                }

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(string? id)
        {
            string returnString = string.Empty;
            SchoolModel schoolModel = new SchoolModel();
            string token = HttpContext.Session.GetString(tokenTxt);
            string url = String.Empty;
            int schoolId = Convert.ToInt32(id);
            if (schoolId > 0)
            {
                url = _apiBaseUrl + $"/api/Schools/GetSchool/{schoolId}";
                var edit_request = new HttpRequestMessage(HttpMethod.Get, url);
                edit_request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // request.Content = contentData;
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage edit_response = await client.SendAsync(edit_request);

                    if (edit_response.StatusCode == HttpStatusCode.OK)
                    {
                        returnString = await edit_response.Content.ReadAsStringAsync();
                        schoolModel = JsonConvert.DeserializeObject<SchoolModel>(returnString);
                    }
                }

            }
            url = _apiBaseUrl + "/api/ClassMasters/GetClassMasters";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // request.Content = contentData;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    returnString = await response.Content.ReadAsStringAsync();
                    var menuList = JsonConvert.DeserializeObject(returnString);
                    JObject json = JObject.Parse(returnString.ToString().Replace("&#xD;&#xA;", Environment.NewLine));
                    ViewBag.Data = json;
                    //_logger.LogError("History not generated!Check please.");
                }

            }
            return View(schoolModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSchool(SchoolModel _schoolModel)
        {

            string stringData = JsonConvert.SerializeObject(_schoolModel);
            string token = HttpContext.Session.GetString(tokenTxt);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            string url = _apiBaseUrl + "/api/Schools/CreateSchool/";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = contentData;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("School not created!Check please.");
                }

            }
            return View("/Views/School/Create.cshtml", new SchoolModel());
        }
    }
}
