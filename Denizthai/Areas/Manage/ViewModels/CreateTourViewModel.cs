using Denizthai.Models;

namespace Denizthai.Areas.Manage.ViewModels
{
    public class CreateTourViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<int> SelectedCategory { get; set; } = new List<int>();

        public IEnumerable<Categorie> CategoryList { get; set; } = new List<Categorie>();
    }
}
