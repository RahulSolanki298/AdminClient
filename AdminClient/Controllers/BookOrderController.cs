using AdminClient.ViewModels;
using AdminClient.ViewModels.Menu;
using AdminClient.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdminClient.Controllers
{
    public class BookOrderController : Controller
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;
        private string appMenu = "AppMenu", userLogCode = "UserLogCode", tokenTxt = "TokenTxt",
           userId = "UserId", schoolId = "SchoolId", imagePath = "ImagePath", fullName = "FullName", roleName = "RoleName";

        public BookOrderController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        public async Task<IActionResult> Index()
        {
            string returnString = string.Empty;
            string token = HttpContext.Session.GetString(tokenTxt);
            //var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            string url = _apiBaseUrl + "/api/BookOrders/GetBookOrder";
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

        public async Task<IActionResult> Create()
        {
            string returnString = string.Empty;
            var schoolid = HttpContext.Session.GetInt32(schoolId);
            string token = HttpContext.Session.GetString(tokenTxt);
            //var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            string url = _apiBaseUrl + "/api/SchoolBookPrices/GetSchoolBookPriceBySchool/" + Convert.ToInt32(schoolid);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // request.Content = contentData;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    returnString = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(returnString.ToString().Replace("&#xD;&#xA;", Environment.NewLine));
                    var menuList = JsonConvert.DeserializeObject<List<SchoolBookPrice>>(json["data"].ToString());

                    ViewBag.Class = menuList;
                    ViewBag.Classcnt = menuList.Count;// Convert.ToInt32(json["recordsTotal"].ToString());
                    HttpContext.Session.SetInt32("BookClassCount", menuList.Count);
                    ViewBag.Data = json;
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookOrder bookOrder, IFormCollection Form)
        {
            string returnString = string.Empty;
            int schoolid = (int)HttpContext.Session.GetInt32(schoolId);
            string token = HttpContext.Session.GetString(tokenTxt);
            int classcnt = (int)HttpContext.Session.GetInt32("BookClassCount");
            for (int i = 1; i <= classcnt; i++)
            {
                BookOrder order = new BookOrder
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = Guid.NewGuid().ToString(),
                    Quantity = Convert.ToInt32(Form["OrderQty_" + i]),
                    OrderAmount = Convert.ToDecimal(Form["Total_" + i]),
                    ClassId = Convert.ToInt32(Form["lbl_" + i]),
                    SchoolId = schoolid,
                    BillTo = "1",
                    OrderStatus = "1"

                };

                string stringData = JsonConvert.SerializeObject(order);
                var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

                string url = _apiBaseUrl + "/api/BookOrders/PostBookOrder";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                request.Content = contentData;
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        //_logger.LogError("History not generated!Check please.");
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
