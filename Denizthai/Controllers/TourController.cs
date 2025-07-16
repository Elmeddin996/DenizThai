using Denizthai.DAL;
using Denizthai.Models;
using Denizthai.ViewModels;
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
        public IActionResult Index(int? categoryId)
{
    var tours = _context.Tours.Include(t => t.Categorie).AsQueryable();

    if (categoryId != null)
    {
        tours = tours.Where(t => t.CategorieId == categoryId);
    }

    var model = new TourFilterViewModel
    {
        Categories = _context.Categories.ToList(),
        SelectedCategoryId = categoryId,
        Tours = tours.ToList()
    };

    return View(model);
}



        public IActionResult Detail(int id)
        {
            Tour tour = _context.Tours.Include(x => x.Categorie).Include(t=>t.TourImages).FirstOrDefault(t=>t.Id == id);
                
            return View(tour);
        }
    }
}
