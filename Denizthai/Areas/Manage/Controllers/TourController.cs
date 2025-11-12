using Denizthai.DAL;
using Denizthai.Helpers;
using Denizthai.Models;
using Denizthai.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Denizthai.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("manage")]
    public class TourController : Controller
    {
        private readonly DenizthaiDbContext _context;

        public IWebHostEnvironment _env;

        public TourController(DenizthaiDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Tours.Include(t => t.Categorie).AsQueryable();

            if (search != null)
                query = query.Where(x => x.NameAz.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Tour>.Create(query, page, 6));
        }

        public IActionResult Create()
        {


            ViewBag.Categories = _context.Categories.ToList();



            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Tour tour)
        {



            if (!_context.Categories.Any(x => x.Id == tour.CategorieId))
            {
                ModelState.AddModelError("CategorieId", "CategorieId is not correct");
                return View();
            }

            tour.Image = FileManager.Save(_env.WebRootPath, "uploads/tours", tour.ImageFile);

            foreach (var img in tour.Images)
            {
                TourImage bookImage = new TourImage
                {
                    ImageName = FileManager.Save(_env.WebRootPath, "uploads/tours", img),
                };
                tour.TourImages.Add(bookImage);
            }

            _context.Tours.Add(tour);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();

            Tour tour = _context.Tours.Include(x => x.TourImages).FirstOrDefault(x => x.Id == id);

            return View(tour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tour tour)
        {

            Tour existTour = _context.Tours.Include(x => x.TourImages).FirstOrDefault(x => x.Id == tour.Id);

            if (existTour == null) return View("Error");


            if (tour.CategorieId != existTour.CategorieId && !_context.Categories.Any(x => x.Id == tour.CategorieId))
            {
                ModelState.AddModelError("CategorieId", "CategorieId is not correct");
                return View();
            }


            string oldImage = null;
            if (tour.ImageFile != null)
            {
                oldImage = tour.Image;

                if (tour.Image == null)
                {
                    tour.Image = FileManager.Save(_env.WebRootPath, "uploads/tours", tour.ImageFile);
                    existTour.Image = tour.Image;
                }
                else
                    tour.Image = FileManager.Save(_env.WebRootPath, "uploads/tours", tour.ImageFile);
            }


            var selectedImageIds = tour.TourImageIds ?? new List<int>();

            var removedImages = existTour.TourImages
                .Where(x => !selectedImageIds.Contains(x.Id))
                .ToList();

            existTour.TourImages.RemoveAll(x => !selectedImageIds.Contains(x.Id));

            foreach (var item in tour.Images)
            {
                TourImage tourImage = new TourImage
                {
                    ImageName = FileManager.Save(_env.WebRootPath, "uploads/tours", item),
                };
                existTour.TourImages.Add(tourImage);
            }


            existTour.NameAz = tour.NameAz;
            existTour.NameRu = tour.NameRu;
            existTour.NameEn = tour.NameEn;
            existTour.DescriptionAz = tour.DescriptionAz;
            existTour.DescriptionRu = tour.DescriptionRu;
            existTour.DescriptionEn = tour.DescriptionEn;
            existTour.LocationAz = tour.LocationAz;
            existTour.LocationRu = tour.LocationRu;
            existTour.LocationEn = tour.LocationEn;
            existTour.DurationAz = tour.DurationAz;
            existTour.DurationRu = tour.DurationRu;
            existTour.DurationEn = tour.DurationEn;
            existTour.Price = tour.Price;
            existTour.SecretWord = tour.SecretWord;
            existTour.DiscountedPrice = tour.DiscountedPrice;
            existTour.CategorieId = tour.CategorieId;
            existTour.IsPopular = tour.IsPopular;

            _context.SaveChanges();


            if (oldImage != null) FileManager.Delete(_env.WebRootPath, "uploads/tours", oldImage);

            if (removedImages.Any())
                FileManager.DeleteAll(_env.WebRootPath, "uploads/tours", removedImages.Select(x => x.ImageName).ToList());


            return RedirectToAction("index");
        }


        public IActionResult Delete(int id)
        {
            Tour tour = _context.Tours
               .Include(t => t.TourImages)
               .FirstOrDefault(t => t.Id == id);

            if (tour == null) return NotFound();

            var removedImages = tour.TourImages
                   .Where(x => !string.IsNullOrEmpty(x.ImageName))
                   .ToList();

            _context.Tours.Remove(tour);
            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath, "uploads/tours", tour.Image);
            if (removedImages.Any())
            {
                FileManager.DeleteAll(_env.WebRootPath, "uploads/tours", removedImages.Select(x => x.ImageName).ToList());
            }
            return RedirectToAction("index");
        }
    }


}

