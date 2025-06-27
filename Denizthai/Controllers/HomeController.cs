using Denizthai.DAL;
using Denizthai.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Denizthai.Controllers
{
    public class HomeController : Controller
    {
        private readonly DenizthaiDbContext _context;
        public HomeController(DenizthaiDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                Locations =_context.Locations.ToList(),
                Tours = _context.Tours.ToList(),
                InstaPhotos = _context.InstaPhotos.ToList(),
                Sliders = _context.Sliders.ToList()
            };
            return View(model);
        }

    }
}
