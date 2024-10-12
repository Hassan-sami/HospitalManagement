
using Hospital.BLL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HospitalManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(Role.Admin.ToString()))
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }
        
        public IActionResult AboutUs()
        {
            return View();
        }


        
    }
}
