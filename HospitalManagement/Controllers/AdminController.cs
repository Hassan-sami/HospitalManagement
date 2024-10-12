using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
