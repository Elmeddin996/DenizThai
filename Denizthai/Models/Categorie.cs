namespace Denizthai.Models
{
    public class Categorie
    {
       public int Id { get; set; }    
        public string NameAz { get; set; }
        public string NameEn { get; set; }
        public string NameRu { get; set; }

        public List<Tour> Tours { get; set; }

    }
}
