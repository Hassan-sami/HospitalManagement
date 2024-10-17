
using Hospital.BLL.Helpers;
using Hospital.BLL.ModelVM;
using Hospital.BLL.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HospitalManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDoctorService doctorService;

        public HomeController(ILogger<HomeController> logger, IDoctorService doctorService)
        {
            _logger = logger;
            this.doctorService = doctorService;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(Role.Admin.ToString()))
            {
                return RedirectToAction("Index", "Admin");
            }
            MainVm vm = new MainVm()
            {
                Doctors = doctorService.GetAllDoctors()
            };

            return View(vm);
        }
        
        public IActionResult AboutUs()
        {
            return View();
        }


        
    }
}
