using Microsoft.AspNetCore.Mvc;

namespace AdminClient.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AcademicYear()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ClassMaster()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Subjects()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SubSubjects()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NoticeType
            ()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VideoCategory()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Videos()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ebook() {
            return View();
        }

        [HttpGet]
        public IActionResult ebookChapter() {
            return View();
        }


        [HttpGet]
        public IActionResult TeachingMedium() {

            return View();
        }

        [HttpGet]
        public IActionResult School()
        {

            return View();
        }


    }
}
