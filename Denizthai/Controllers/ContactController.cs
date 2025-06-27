using Denizthai.DAL;
using Denizthai.Models;
using Denizthai.Services;
using Denizthai.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Denizthai.Controllers
{
	public class ContactController : Controller
	{
		private readonly DenizthaiDbContext _context;
        private readonly IEmailSender _emailSender;

        public ContactController(DenizthaiDbContext context, IEmailSender emailSender)
		{
			_context = context;
            _emailSender = emailSender;
        }
		public IActionResult Index()
		{
            MenuViewModel model = new MenuViewModel
            {
            Settings  = _context.Settings.FirstOrDefault(),
            };

			return View(model);
		}

        [HttpPost]
        public IActionResult Index(ContactPageMessageViewModel vm)
        {
            Settings settings = _context.Settings.FirstOrDefault();

            string messageBody = $@"
             <p><strong>Ad Soyad:</strong> {vm.Fullname}</p>
             <p><strong>Telefon:</strong> {vm.Phone}</p>
             <p><strong>E-Mail:</strong> {vm.Email}</p>
             <p><strong>Mesaj:</strong> {vm.Message}</p>";

            _emailSender.Send(settings.Email, "Saytadan Bir Başa Mesaj", messageBody);
            return View(settings);
        }
    }
}
