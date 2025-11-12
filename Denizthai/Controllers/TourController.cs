using Denizthai.DAL;
using Denizthai.Models;
using Denizthai.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Denizthai.Controllers
{
    public class TourController : Controller
    {
        private readonly DenizthaiDbContext _context;
        public TourController(DenizthaiDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? categoryId, bool popular = false)
        {
            var tours = _context.Tours.Include(t => t.Categorie).AsQueryable();

            if (categoryId != null)
            {
                tours = tours.Where(t => t.CategorieId == categoryId);
            }

            if (popular)
            {
                tours = tours.Where(t => t.IsPopular);
            }

            var model = new TourFilterViewModel
            {
                Categories = _context.Categories.ToList(),
                SelectedCategoryId = categoryId,
                Tours = tours.ToList(),
                 IsPopular = popular
            };

            return View(model);
        }



        public IActionResult Detail(int id)
        {
            Tour tour = _context.Tours.Include(x => x.Categorie).Include(t => t.TourImages).FirstOrDefault(t => t.Id == id);

            return View(tour);
        }
    }
}
