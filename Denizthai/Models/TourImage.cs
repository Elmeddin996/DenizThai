namespace Denizthai.Models
{
    public class TourImage
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string ImageName { get; set; }
        public Tour Tour { get; set; }
    }
}
