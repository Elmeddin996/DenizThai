using Denizthai.Web.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Denizthai.Models
{
    public class InstaPhoto
    {
        public int Id { get; set; }
        public string Image { get; set; }

        [MaxFileSize(100 * 1024 * 1024)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
