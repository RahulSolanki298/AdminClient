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
            return View();
        }

        [HttpPost]
        public IActionResult SetChangeSchoolSession(SessionManager session)
        {
            HttpContext.Session.SetString(session.IdKey, session.IdValue);
            HttpContext.Session.SetString(session.NameKey, session.NameValue);
            return Json(HttpContext.Session.GetString(session.IdValue));
        }
    }
}
