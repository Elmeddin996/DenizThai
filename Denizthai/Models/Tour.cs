using Denizthai.Web.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Denizthai.Models
{
    public class Tour

    {
       public int Id { get; set; }
        public string NameAz { get; set; }
        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public string DescriptionAz { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionRu { get; set; }
        public string LocationAz { get; set; }
        public string LocationEn { get; set; }
        public string LocationRu { get; set; }
        public string DurationAz { get; set; }
        public string DurationEn { get; set; }
        public string DurationRu { get; set; }
        
        public string Price { get; set; }
        public string DiscountedPrice { get; set;}
        public int CategorieId { get; set; }
        public bool IsPopular { get; set; }
        public string Image { get; set; }

        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        [NotMapped]
        public List<int> TourImageIds { get; set; } = new List<int>();

        public List<TourImage> TourImages { get; set; } = new List<TourImage>();

        public Categorie Categorie { get; set; }




    }
}
