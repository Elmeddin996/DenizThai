using Denizthai.DAL;
using Denizthai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Denizthai.Controllers
{
    public class TourController : Controller
    {
        private readonly DenizthaiDbContext _context;
        public TourController( DenizthaiDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Tour>tours = _context.Tours.Include(x=>x.Categorie).ToList();
            return View(tours);
        }

        public IActionResult Detail(int id)
        {
            Tour tour = _context.Tours.Include(x => x.Categorie).Include(t=>t.TourImages).FirstOrDefault(t=>t.Id == id);
                
            return View(tour);
        }
    }
}
