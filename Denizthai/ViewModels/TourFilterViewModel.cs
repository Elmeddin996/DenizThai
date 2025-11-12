using Denizthai.Models;

namespace Denizthai.ViewModels
{
    public class TourFilterViewModel
    {
        public int? SelectedCategoryId { get; set; }
        public List<Categorie> Categories { get; set; }
        public List<Tour> Tours { get; set; }
        public bool IsPopular { get; set; }
    }
}
