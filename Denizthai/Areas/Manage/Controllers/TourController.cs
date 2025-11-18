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

            var tour = new Tour
            {
                CategoryIds = new List<int>()
            };

            return View(tour);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Tour tour)
        {



            var selected = tour.CategoryIds ?? new List<int>();

            if (!selected.Any())
            {
                ModelState.AddModelError("CategoryIds", "Please select at least one category.");
                ViewBag.Categories = _context.Categories.ToList();
                return View(tour);
            }

            if (selected.Any(id => !_context.Categories.Any(c => c.Id == id)))
            {
                ModelState.AddModelError("CategoryIds", "One or more selected categories are invalid.");
                ViewBag.Categories = _context.Categories.ToList();
                return View(tour);
            }

            tour.TourCategories = tour.TourCategories ?? new List<TourCategory>();

            foreach (var categoryId in selected)
            {
                TourCategory tourCategory = new TourCategory
                {
                    CategoryId = categoryId,
                };

                tour.TourCategories.Add(tourCategory);
            }

            tour.Image = FileManager.Save(_env.WebRootPath, "uploads/tours", tour.ImageFile);

            foreach (var img in tour.Images)
            {
                TourImage tourImage = new TourImage
                {
                    ImageName = FileManager.Save(_env.WebRootPath, "uploads/tours", img),
                };
                tour.TourImages.Add(tourImage);
            }

            _context.Tours.Add(tour);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();

            Tour tour = _context.Tours
                .Include(x => x.TourImages)
                .Include(x => x.TourCategories)
                .FirstOrDefault(x => x.Id == id);

            tour.CategoryIds = tour.TourCategories.Select(x => x.CategoryId).ToList();

            return View(tour);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tour tour)
        {
            Tour existTour = _context.Tours
                .Include(x => x.TourImages)
                .Include(x => x.TourCategories)
                .FirstOrDefault(x => x.Id == tour.Id);

            if (existTour == null) return View("Error");


            var selectedCategoryIds = tour.CategoryIds ?? new List<int>();

            
            var validCategoryIds = _context.Categories
                .Where(c => selectedCategoryIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToList();

           
            if (selectedCategoryIds.Count != validCategoryIds.Count)
            {
                ModelState.AddModelError("CategoryIds", "Seçilmiş kateqoriyalardan bəziləri mövcud deyil.");
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.SelectedCategories = selectedCategoryIds;
                return View(tour);
            }

           
            existTour.TourCategories.RemoveAll(x => !validCategoryIds.Contains(x.CategoryId));

           
            var existingIds = existTour.TourCategories.Select(x => x.CategoryId).ToHashSet();
            var toAdd = validCategoryIds.Where(id => !existingIds.Contains(id));

            foreach (var id in toAdd)
            {
                existTour.TourCategories.Add(new TourCategory { CategoryId = id });
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

