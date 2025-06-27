using Denizthai.DAL;
using Denizthai.Helpers;
using Denizthai.Models;
using Denizthai.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Denizthai.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]

    [Area("manage")]
    public class InstaPhotoController : Controller
    {
        private readonly DenizthaiDbContext _context;

        public IWebHostEnvironment _env { get; }

        public InstaPhotoController(DenizthaiDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.InstaPhotos.AsQueryable();


            return View(PaginatedList<InstaPhoto>.Create(query, page, 6));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InstaPhoto instaPhoto)
        {

            if (instaPhoto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }


            instaPhoto.Image = FileManager.Save(_env.WebRootPath, "uploads/instaphotos", instaPhoto.ImageFile);


            _context.InstaPhotos.Add(instaPhoto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            InstaPhoto instaPhoto = _context.InstaPhotos.Find(id);

            if (instaPhoto == null) return View("Error");

            return View(instaPhoto);
        }

        [HttpPost]
        public IActionResult Edit(InstaPhoto instaPhoto)
        {
            InstaPhoto existPhoto = _context.InstaPhotos.Find(instaPhoto.Id);

            if (existPhoto == null) return View("Error");

            string oldFileName = null;
            if (instaPhoto.ImageFile != null)
            {
                oldFileName = existPhoto.Image;

                if (instaPhoto.Image == null)
                {
                    instaPhoto.Image = FileManager.Save(_env.WebRootPath, "uploads/instaphotos", instaPhoto.ImageFile);
                    existPhoto.Image = instaPhoto.Image;
                }
                else
                    instaPhoto.Image = FileManager.Save(_env.WebRootPath, "uploads/instaphotos", instaPhoto.ImageFile);
            }

            _context.SaveChanges();

            if (oldFileName != null)
                FileManager.Delete(_env.WebRootPath, "uploads/instaphotos", oldFileName);

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            InstaPhoto instaPhoto = _context.InstaPhotos.Find(id);

            if (instaPhoto == null) return StatusCode(404);

            _context.InstaPhotos.Remove(instaPhoto);
            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath, "uploads/instaphotos", instaPhoto.Image);

            return StatusCode(200);
        }
    }
}
