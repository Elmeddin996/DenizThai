namespace Denizthai.Models
{
    public class TourCategory
    {
        public int Id { get; set; }

        public int TourId { get; set; }
        public int CategoryId { get; set; } 

        public virtual Tour Tour { get; set; }
        public virtual Categorie Category { get; set; } 
    }
}
