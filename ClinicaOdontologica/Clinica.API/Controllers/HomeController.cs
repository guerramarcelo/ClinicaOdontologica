using Microsoft.AspNetCore.Mvc;

namespace Clinica.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
