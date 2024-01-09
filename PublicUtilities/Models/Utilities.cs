using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Models
{
    public class Utilities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Indicators> Indicators { get; set; }
    }
}
