using Denizthai.Web.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Denizthai.Models
{
    public class InstaPhoto
    {
        public int Id { get; set; }
        public string Image { get; set; }

        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
