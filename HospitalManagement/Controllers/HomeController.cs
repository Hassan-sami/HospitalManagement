
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
            // Example data
            ViewBag.NumberOfDoctors = 5; // replace with actual data
            ViewBag.NumberOfPatients = 20; // replace with actual data
            ViewBag.NumberOfAppointments = 10; // replace with actual data
            ViewBag.NumberOfMedicalRecords = 15; // replace with actual data
            ViewBag.NewAppointments = 2; // replace with actual data
            ViewBag.TodayPatients = 3; // replace with actual data

            return View(); // this will render the view with the layout
        }
        public IActionResult Privacy()
        {
            return View();
        }


        
    }
}
