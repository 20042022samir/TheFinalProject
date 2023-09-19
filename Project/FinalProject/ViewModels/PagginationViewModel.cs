namespace FinalProject.ViewModels
{
    public class PagginationViewModel<W>
    {
        public List<W> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
