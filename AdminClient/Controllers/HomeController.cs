using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdminClient.ViewModels;
using AdminClient.ViewModels.Menu;
using AdminClient.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AdminClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        private string appMenu = "AppMenu", userLogCode = "UserLogCode", tokenTxt = "TokenTxt",
            userId="UserId", schoolId = "SchoolId",imagePath="ImagePath",fullName="FullName",roleName="RoleName";
        

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }

        [HttpGet]
        public IActionResult Login()
        {         
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                string apiUrl = _apiBaseUrl + "/api/Users/GetLoginInfo/" + username + "/" + password;
                using (HttpClient client = new HttpClient())
                {
                    using (var response = await client.GetAsync(apiUrl))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var objUser = JsonConvert.DeserializeObject<UserInfoWithToken>(apiResponse);
                            if (objUser != null)
                            {
                                string urlMenu = _apiBaseUrl + "/api/Menu/GetAppMenu/" + objUser.obj.UserRoleId;                             
                                var request = new HttpRequestMessage(HttpMethod.Get, urlMenu);
                                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", objUser.token);
                               
                                using (var responseMenu = await client.SendAsync(request))
                                {
                                    if (responseMenu.StatusCode == HttpStatusCode.OK)
                                    {
                                        string returnMenuString= await responseMenu.Content.ReadAsStringAsync();
                                        var menuList = JsonConvert.DeserializeObject<List<MenuDisplay>>(returnMenuString);                                      
                                        string menuString = listToSidebar(menuList);

                                        var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                                        var logCode = objUser.obj.UserId +"-"+ dateTime;

                                        HttpContext.Session.SetString(appMenu, menuString);
                                        HttpContext.Session.SetInt32(userId, objUser.obj.UserId);
                                        HttpContext.Session.SetString(userLogCode, logCode);
                                        HttpContext.Session.SetString(tokenTxt, objUser.token);
                                        HttpContext.Session.SetString(imagePath, objUser.obj.ImagePath==null?"": objUser.obj.ImagePath);
                                        HttpContext.Session.SetString(fullName, objUser.obj.FirstName);
                                        HttpContext.Session.SetString(roleName, objUser.obj.RoleName);
                                        HttpContext.Session.SetInt32(schoolId, Convert.ToInt32(objUser.obj.SchoolId));
                                        HttpContext.Session.SetString(SessionKeys.httpSchoolId,Convert.ToString(objUser.obj.SchoolId));

                                        CreateLoginHistory(objUser.obj.UserId, logCode, objUser.token);
                                        return RedirectToAction("Index","DashBoard");
                                    }
                                    else if (responseMenu.StatusCode == HttpStatusCode.Forbidden)
                                    {
                                        ViewBag.ErrorMessage = "No Permission!Contact to Admin.";                                       
                                    }
                                    else if (responseMenu.StatusCode == HttpStatusCode.Unauthorized)
                                    {
                                        ViewBag.ErrorMessage = "Token expired!Try Again.";                                       
                                    }                                  
                                }
                            }
                        }
                        else if (response.StatusCode == HttpStatusCode.NoContent)
                        {
                            ViewBag.ErrorMessage = "Incorrect Username/Password ! Please try again.";
                        }                        
                        else
                        {
                            ViewBag.ErrorMessage = "Invalid request!Please try again";
                        }

                    }
                }
            }
            catch(Exception exception)
            {                           
                ViewBag.ErrorMessage = "Something is going wrong!Please try again";
                _logger.LogError(exception.Message);
            }           
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {              
                string logCode = HttpContext.Session.GetString(userLogCode);                
                string token = HttpContext.Session.GetString(tokenTxt);
                HttpContext.Session.Clear();
                string url = _apiBaseUrl + "/api/Users/UpdateLoginHistory/" + logCode;
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var client = new HttpClient())
                {
                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return RedirectToAction("Login");
                        }

                    }

                }
            }
            catch (Exception exception)
            {
                HttpContext.Session.Clear();
                _logger.LogError(exception.Message);
            }           
            return View("Login");
        }

        private string listToSidebar(List<MenuDisplay> listOfMenu)
        {
            List<MenuDisplay> parentMenusList = listOfMenu.Where(q => q.ParentID == 0).ToList<MenuDisplay>();
            var sb = new StringBuilder();
            string unorderedList = GenerateSidebar(parentMenusList, listOfMenu, sb);
            unorderedList = "<ul class='navbar-nav flex-column mt-4'>" + unorderedList.Substring(33, unorderedList.Length - 33);

            return unorderedList;
        }

        private string GenerateSidebar(List<MenuDisplay> parentMenusList, List<MenuDisplay> listOfMenu, StringBuilder sb)
        {
            sb.Append("<ul class='flex-column nav pl-4'>");

            if (parentMenusList.Count > 0)
            {
                foreach (var parentMenu in parentMenusList)
                {
                    string handler = parentMenu.URL != null ? parentMenu.URL.ToString() : "";
                    string menuText = parentMenu.MenuTitle != null ? parentMenu.MenuTitle.ToString() : "";
                    string iconClass = parentMenu.IconClass != null ? parentMenu.IconClass.ToString() : "";

                    string line = "";
                    if (parentMenu.ParentID.ToString() == "0" && parentMenu.IsSubMenu.ToString() == "1")
                    {
                        line = String.Format(@"<li class='nav-item'><a class='nav-link collapsed text-white p-3 mb-1 sidebar-link' href='#{2}' data-toggle='collapse' data-target='#{2}'><i class='{1} text-light fa-lg mr-3'></i>{0}<i class='fas fa-caret-left fa-lg float-right'></i></a>", menuText, iconClass,parentMenu.MenuID.ToString());
                    }
                    else if (parentMenu.ParentID.ToString() == "0" && parentMenu.IsSubMenu.ToString() == "0")
                    {
                        line = String.Format(@"<li class='nav-item'><a href='{0}' class='nav-link text-white p-3 mb-1'><i class='{1} text-light fa-lg mr-3'></i>{2}</a>", handler, iconClass, menuText);
                    }
                    else if (parentMenu.ParentID.ToString() != "0" && parentMenu.IsSubMenu.ToString() == "1")
                    {
                        line = String.Format(@"<li class='nav-item'><a class='nav-link collapsed text-white p-3 mb-1 sidebar-link' href='#{1}' data-toggle='collapse' data-target='#{1}'><i class='far fa-dot-circle text-light fa-lg mr-3'></i>{0}<i class='fas fa-caret-left fa-lg float-right'></i></a>", menuText, parentMenu.MenuID.ToString());
                    }
                    else
                    {                      
                        line = String.Format(@"<li class='nav-item'><a href='{0}' class='nav-link text-white py-3 mb-1 sidebar-link'><i class='far fa-circle text-light mr-1'></i>{1}</a>", handler, menuText);
                    }
                    sb.Append(line);

                    string pid = parentMenu.MenuID.ToString();
                    string parentId = parentMenu.ParentID.ToString();

                    List<MenuDisplay> subMenu = listOfMenu.Where(q => q.ParentID.ToString() == pid).ToList<MenuDisplay>();
                    if (subMenu.Count > 0)
                    {
                        var subMenuBuilder = new StringBuilder();
                        string childMenu = "<div class='collapse' id="+ pid + " aria-expanded='false'>";
                        sb.Append(childMenu);
                        sb.Append(GenerateSidebar(subMenu, listOfMenu, subMenuBuilder));
                        sb.Append("</div>");
                    }
                    sb.Append("</li>");
                }
            }

            sb.Append("</ul>");
            
            return sb.ToString();
        }

        private async void CreateLoginHistory(int userId, string logCode, string token)
        {
            var logHistory = new LogHistory
            {
                LogCode = logCode,
                LogDate = DateTime.Now,
                UserId = userId,
                LogInTime = DateTime.Now
            };

            string stringData = JsonConvert.SerializeObject(logHistory);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            string url = _apiBaseUrl + "/api/Users/CreateLoginHistory/";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = contentData;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("History not generated!Check please.");
                }               
            }
        }
    }
}
