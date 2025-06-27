using Denizthai.Models;
using Denizthai.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Denizthai.DAL
{
    public class DenizthaiDbContext : IdentityDbContext
    {

        public DenizthaiDbContext(DbContextOptions<DenizthaiDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourImage> TourImages { get; set; }
        public DbSet<InstaPhoto> InstaPhotos { get; set; }

    }
}
