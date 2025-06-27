using Denizthai.DAL;
using Denizthai.Models;


namespace Denizthai.Services
{
    public class LayoutService
    {
        private readonly DenizthaiDbContext _contex;

        public LayoutService(DenizthaiDbContext contex)
        {
            _contex = contex;
        }

        public List<Settings> GetSettings()
        {
            return _contex.Settings.ToList();
        }
       
       
    }
}
