using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Denizthai.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult ChangeLanguage(string lang, string returnUrl = "/")
        {
            if (!string.IsNullOrEmpty(lang))
            {
                string culture = lang switch
                {
                    "az" => "az",
                    "en" => "en",
                    "ru" => "ru",
                    _ => "az" // Default
                };

                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            return LocalRedirect(returnUrl);
        }
    }
}
