using Denizthai.DAL;
using Denizthai.Models;
using Microsoft.AspNetCore.Mvc;

namespace Denizthai.Controllers
{
    public class FaqController : Controller
    {
        private readonly DenizthaiDbContext _context;

        public FaqController(DenizthaiDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Faq> Faqs = _context.Faqs.ToList();
            return View(Faqs);
        }
    }
}
