using Denizthai.DAL;
using Denizthai.Models;
using Denizthai.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DenizthaiDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 8;
    opt.SignIn.RequireConfirmedEmail = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<DenizthaiDbContext>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddSingleton<LocService>();
builder.Services.AddScoped<LayoutService>();



builder.Services.AddHttpContextAccessor();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.HttpContext.Request.Path.Value.StartsWith("/manage"))
        {
            var redirectUri = new Uri(context.RedirectUri);
            context.Response.Redirect("/manage/account/login" + redirectUri.Query);
        }
        else
        {
            var redirectUri = new Uri(context.RedirectUri);
            context.Response.Redirect("/account/login" + redirectUri.Query);
        }

        return Task.CompletedTask;
    };
});

// 1. Localization xidmətlərini əlavə et
//builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddLocalizationService();

// 2. MVC controller və view-lar üçün localization dəstəyi əlavə et
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();


var app = builder.Build();

app.AddLocalization();

//var supportedCultures = new[] { "az", "en", "ru" };

//var localizationOptions = new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture("az"),
//    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
//    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
//};

//localizationOptions.RequestCultureProviders = new[]
//{
//    new CookieRequestCultureProvider()
//};


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseRequestLocalization(localizationOptions);
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
