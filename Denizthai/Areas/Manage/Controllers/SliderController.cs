﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Denizthai.DAL;
using Denizthai.Helpers;
using Denizthai.Models;
using Denizthai.ViewModels;
using System.Data;
using Denizthai.Web.Models;

namespace Denizthai.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]

    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly DenizthaiDbContext _context;

        public IWebHostEnvironment _env { get; }

        public SliderController(DenizthaiDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Sliders.OrderBy(x=>x.Order).AsQueryable();

            
            return View(PaginatedList<Slider>.Create(query, page, 6));
        }

        public IActionResult Create()
        {
            ViewBag.NextOrder = _context.Sliders.Any() ? _context.Sliders.Max(x => x.Order) + 1 : 1;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            ViewBag.NextOrder = slider.Order;

            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }

            foreach (var item in _context.Sliders.Where(x => x.Order >= slider.Order))
                item.Order++;

            slider.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
           

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null) return View("Error");

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            Slider existSlider = _context.Sliders.Find(slider.Id);

            if (existSlider == null) return View("Error");

            string  oldFileName = null;
            if (slider.ImageFile != null)
            {
                oldFileName = existSlider.Image;

                if (slider.Image == null)
                {
                    slider.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
                    existSlider.Image = slider.Image;
                }
                else
                    slider.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }
           


            existSlider.Order = slider.Order;
            existSlider.TitleAz = slider.TitleAz;
            existSlider.TitleEn = slider.TitleEn;
            existSlider.TitleRu = slider.TitleRu;
			existSlider.BtnUrl = slider.BtnUrl;
            existSlider.ButtonTextAz = slider.ButtonTextAz;
            existSlider.ButtonTextEn = slider.ButtonTextEn;
            existSlider.ButtonTextRu = slider.ButtonTextRu;
			existSlider.DescriptionAz = slider.DescriptionAz;
			existSlider.DescriptionEn = slider.DescriptionEn;
			existSlider.DescriptionRu = slider.DescriptionRu;

			_context.SaveChanges();

            if (oldFileName != null)
                FileManager.Delete(_env.WebRootPath, "uploads/sliders", oldFileName);
            
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null) return StatusCode(404);

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath, "uploads/sliders", slider.Image);
           
            return StatusCode(200);
        }
    }
}
