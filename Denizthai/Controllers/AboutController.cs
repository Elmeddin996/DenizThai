using Denizthai.DAL;
using Denizthai.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace Denizthai.Controllers
{
	public class AboutController : Controller
	{
		private readonly DenizthaiDbContext _context;

		public AboutController(DenizthaiDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
		
			MenuViewModel model = new MenuViewModel
			{
				Settings = _context.Settings.FirstOrDefault(),
			};

			return View(model);
		}
	}

}
