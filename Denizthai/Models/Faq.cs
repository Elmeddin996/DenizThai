using Denizthai.Web.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Denizthai.Models
{
    public class Faq
    {
        public int Id { get; set; }
        public string QuestionAz { get; set; }
        public string QuestionRu { get; set; }
        public string QuestionEn { get; set; }
        public string AnswerAz { get; set; }
        public string AnswerRu { get; set; }
        public string AnswerEn { get; set; }
        public string? Image { get; set; }

        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
