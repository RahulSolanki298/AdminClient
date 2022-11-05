using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Text;
using AdminClient.ViewModels;

namespace AdminClient.Controllers
{
    public class TeachingPlanController : Controller
    {
        private readonly ILogger<TeachingPlanController> _logger;
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;
        private string appMenu = "AppMenu", userLogCode = "UserLogCode", tokenTxt = "TokenTxt",
           userId = "UserId", schoolId = "SchoolId", imagePath = "ImagePath", fullName = "FullName", roleName = "RoleName";

        public TeachingPlanController(IConfiguration configuration, ILogger<TeachingPlanController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        public async Task<IActionResult> Index()
        {
            string returnString = string.Empty;
            string token = HttpContext.Session.GetString(tokenTxt);
            string url = _apiBaseUrl + "/api/TeachingPlans/GetTeachingPlan";
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
            TeachingPlanData teachingModel = new TeachingPlanData();
            string token = HttpContext.Session.GetString(tokenTxt);
            ViewBag.AcadamyId = HttpContext.Session.GetString(SessionKeys.httpAcYearId);        
            int teachingPlanId = Convert.ToInt32(id);
            if (teachingPlanId > 0)
            {
                string url = _apiBaseUrl + $"/api/TeachingPlans/GetTeachingPlan/{teachingPlanId}";
                var edit_request = new HttpRequestMessage(HttpMethod.Get, url);
                edit_request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage edit_response = await client.SendAsync(edit_request);

                    if (edit_response.StatusCode == HttpStatusCode.OK)
                    {
                        string returnString = await edit_response.Content.ReadAsStringAsync();
                        teachingModel = JsonConvert.DeserializeObject<TeachingPlanData>(returnString);
                    }
                }
            }
            string ebookUrl= _apiBaseUrl + $"/api/eBookChapters/GeteBookChapter";
            var eBook_request = new HttpRequestMessage(HttpMethod.Get, ebookUrl);
            eBook_request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using(HttpClient ebookClient=new HttpClient())
            {
                HttpResponseMessage eBook_response = await ebookClient.SendAsync(eBook_request);

                string returnString = await eBook_response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(returnString.ToString().Replace("&#xD;&#xA;", Environment.NewLine));
                ViewBag.eBookChaptersData = json;
            }

            string academyUrl = _apiBaseUrl + $"/api/AcademyYears/GetAcademyYears";
            var academy_request = new HttpRequestMessage(HttpMethod.Get, academyUrl);
            academy_request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (HttpClient academyClient = new HttpClient())
            {
                HttpResponseMessage academy_response = await academyClient.SendAsync(academy_request);

                string returnString = await academy_response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(returnString.ToString().Replace("&#xD;&#xA;", Environment.NewLine));
                ViewBag.AcademyYearData = json;
            }
            return View(teachingModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTeachingPlan(TeachingPlanData _teachingModel)
        {
            string stringData = JsonConvert.SerializeObject(_teachingModel);
            string token = HttpContext.Session.GetString(tokenTxt);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            string url = _apiBaseUrl + "/api/TeachingPlans/PostTeachingPlan/";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = contentData;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Teaching plan not created!Check please.");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
