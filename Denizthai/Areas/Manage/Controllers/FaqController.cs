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
    public class FaqController : Controller
    {
        private readonly DenizthaiDbContext _context;
        private readonly IWebHostEnvironment _env;


        public FaqController(DenizthaiDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Faqs.AsQueryable();

            if (search != null)
                query = query.Where(x => x.QuestionAz.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Faq>.Create(query, page, 6));
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Faq faq)
        {

            if (faq.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }


            faq.Image = FileManager.Save(_env.WebRootPath, "uploads/faqphotos", faq.ImageFile);


            _context.Faqs.Add(faq);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Faq faq = _context.Faqs.Find(id);

            if (faq == null) return View("Error");

            return View(faq);
        }

        [HttpPost]
        public IActionResult Edit(Faq faq)
        {

            Faq existFaq = _context.Faqs.Find(faq.Id);

            if (existFaq == null) return View("Error");

            if (faq.QuestionAz != existFaq.QuestionAz && _context.Faqs.Any(x => x.QuestionAz == faq.QuestionAz))
            {
                ModelState.AddModelError("QuestionAz", "Faq Name is already taken");
                return View();
            }

            string oldFileName = null;
            if (faq.ImageFile != null)
            {
                oldFileName = existFaq.Image;

                if (faq.Image == null)
                {
                    faq.Image = FileManager.Save(_env.WebRootPath, "uploads/faqphotos", faq.ImageFile);
                    existFaq.Image = faq.Image;
                }
                else
                    faq.Image = FileManager.Save(_env.WebRootPath, "uploads/faqphotos", faq.ImageFile);
            }

            existFaq.QuestionAz = faq.QuestionAz;
            existFaq.QuestionRu = faq.QuestionRu;
            existFaq.QuestionEn = faq.QuestionEn;
            existFaq.AnswerAz = faq.AnswerAz;
            existFaq.AnswerRu = faq.AnswerRu;
            existFaq.AnswerEn = faq.AnswerEn;

            _context.SaveChanges();

            if (oldFileName != null)
                FileManager.Delete(_env.WebRootPath, "uploads/faqphotos", oldFileName);

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Faq faq = _context.Faqs.Find(id);

            if (faq == null) return StatusCode(404);

            _context.Faqs.Remove(faq);
            _context.SaveChanges();

            return StatusCode(200);
        }
    }
}
