using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicUtilities.Models
{
    public class Notifications
    {
        public int Id { get; set; }
        public string? Streets { get; set; }
        public string? House { get; set; }
        public string? Apartment { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
