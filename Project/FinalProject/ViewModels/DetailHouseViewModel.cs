using FinalProject.Core.Entities;

namespace FinalProject.ViewModels
{
    public class DetailHouseViewModel
    {
        public House? house { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
        public Comment? Comment { get; set; }
        public IEnumerable<House>? ExtraHouses { get; set; }
        public ReplyComment? ReplyComment { get; set; }
        public IEnumerable<ReplyComment>? ReplyComments { get; set; }
    }
}
