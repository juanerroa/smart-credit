using Microsoft.AspNetCore.Mvc;

namespace SmartCredit.FrontEnd.WebApp.Controllers
{
    public class HealthCheck : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
