using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string ImageUrl { get; set; }
    }
}
