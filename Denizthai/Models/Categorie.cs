using Denizthai.Web.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Denizthai.Models
{
    public class Categorie
    {
       public int Id { get; set; }    
        public string NameAz { get; set; }
        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public string? Image { get; set; }

        [MaxFileSize(100 * 1024 * 1024)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public List<Tour> Tours { get; set; }
        public List<TourCategory> TourCategories { get; set; }=new List<TourCategory>();

    }
}
