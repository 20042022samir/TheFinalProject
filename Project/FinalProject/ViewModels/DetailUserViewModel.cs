using FinalProject.Core.Entities;

namespace FinalProject.ViewModels
{
    public class DetailUserViewModel
    {
        public AppUser user { get; set; }
        public IEnumerable<House> houses { get; set; }
    }
}
