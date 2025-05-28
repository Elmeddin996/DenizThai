using Microsoft.AspNetCore.Mvc;

namespace Denizthai.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }

    }
}
