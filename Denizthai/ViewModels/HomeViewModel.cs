using Denizthai.Models;
using Denizthai.Web.Models;

namespace Denizthai.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set;}
        public List<Tour> Tours { get; set;}
        public List<Location> Locations { get; set;}
        public List<InstaPhoto> InstaPhotos { get; set;}
    }
}
