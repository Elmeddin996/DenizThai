using Denizthai.DAL;
using Denizthai.Helpers;
using Denizthai.Models;
using Denizthai.ViewModels;
using Denizthai.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Denizthai.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("manage")]
    public class CategorieController : Controller
    {
        private readonly DenizthaiDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategorieController(DenizthaiDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Categories.AsQueryable();

            if (search != null)
                query = query.Where(x => x.NameAz.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Categorie>.Create(query, page, 6));
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Categorie categorie)
        {
            categorie.Image = FileManager.Save(_env.WebRootPath, "uploads/categories", categorie.ImageFile);


            _context.Categories.Add(categorie);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Categorie categorie = _context.Categories.Find(id);

            if (categorie == null) return View("Error");

            return View(categorie);
        }

        [HttpPost]
        public IActionResult Edit(Categorie categorie)
        {

            Categorie existCategorie = _context.Categories.Find(categorie.Id);

            if (existCategorie == null) return View("Error");


            if (categorie.NameAz != existCategorie.NameAz && _context.Categories.Any(x => x.NameAz == categorie.NameAz))
            {
                ModelState.AddModelError("NameAz", "Category Name is already taken");
                return View();
            }

            string oldFileName = null;
            if (categorie.ImageFile != null)
            {
                oldFileName = existCategorie.Image;

                if (categorie.Image == null)
                {
                    categorie.Image = FileManager.Save(_env.WebRootPath, "uploads/categories", categorie.ImageFile);
                    existCategorie.Image = categorie.Image;
                }
                else
                    categorie.Image = FileManager.Save(_env.WebRootPath, "uploads/categories", categorie.ImageFile);
            }


            existCategorie.NameAz= categorie.NameAz;
            existCategorie.NameRu= categorie.NameRu;
            existCategorie.NameEn= categorie.NameEn;

            _context.SaveChanges();

            if (oldFileName != null)
                FileManager.Delete(_env.WebRootPath, "uploads/categories", oldFileName);


            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Categorie categorie = _context.Categories.Find(id);

            if (categorie == null) return StatusCode(404);

            _context.Categories.Remove(categorie);
            _context.SaveChanges();

            return StatusCode(200);
        }
    }
}

