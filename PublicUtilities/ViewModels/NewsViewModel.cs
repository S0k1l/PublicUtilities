using PublicUtilities.Models;

namespace PublicUtilities.ViewModels
{
    public class NewsViewModel
    {
        public IEnumerable<News> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
