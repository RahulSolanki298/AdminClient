using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminClient.AppHelper;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdminClient.Controllers
{
    [CheckSession]
    public class DashBoardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var defaultSchoolSession = HttpContext.Session.GetString("httpSchool");
            var defaultAcYearSession = HttpContext.Session.GetString("httpAcYear");
            if (defaultSchoolSession == null)
            {
                HttpContext.Session.SetString("httpSchool", "3");
            }

            if (defaultAcYearSession == null)
            {
                HttpContext.Session.SetString("httpAcYear", "1");
            }
            return View();
        }

        [HttpPost]
        public IActionResult SetChangeSchoolSession(SessionManager session)
        {
            HttpContext.Session.SetString(session.key, session.value);
            return Json(HttpContext.Session.GetString(session.value));
        }
    }
}
