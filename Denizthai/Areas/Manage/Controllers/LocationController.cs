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
    public class LocationController : Controller
    {
        private readonly DenizthaiDbContext _context;

        public IWebHostEnvironment _env { get; }

        public LocationController(DenizthaiDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Locations.AsQueryable();


            return View(PaginatedList<Location>.Create(query, page, 6));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Location location)
        {

            if (location.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }

            
            location.Image = FileManager.Save(_env.WebRootPath, "uploads/locations", location.ImageFile);


            _context.Locations.Add(location);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Location location = _context.Locations.Find(id);

            if (location == null) return View("Error");

            return View(location);
        }

        [HttpPost]
        public IActionResult Edit(Location location)
        {
            Location existLocation = _context.Locations.Find(location.Id);

            if (existLocation == null) return View("Error");

            string oldFileName = null;
            if (location.ImageFile != null)
            {
                oldFileName = existLocation.Image;

                if (location.Image == null)
                {
                    location.Image = FileManager.Save(_env.WebRootPath, "uploads/locations", location.ImageFile);
                    existLocation.Image = location.Image;
                }
                else
                    location.Image = FileManager.Save(_env.WebRootPath, "uploads/locations", location.ImageFile);
            }



           
            existLocation.NameAz = location.NameAz;
            existLocation.NameRu = location.NameRu;
            existLocation.NameEn = location.NameEn;

            _context.SaveChanges();

            if (oldFileName != null)
                FileManager.Delete(_env.WebRootPath, "uploads/locations", oldFileName);

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Location location = _context.Locations.Find(id);

            if (location == null) return StatusCode(404);

            _context.Locations.Remove(location);
            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath, "uploads/locations", location.Image);

            return StatusCode(200);
        }
    }
}
