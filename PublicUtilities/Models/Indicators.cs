using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicUtilities.Models
{
    public class Indicators
    {
        public int Id { get; set; }
        public int PlacesOfResidenceId { get; set; }
        [ForeignKey("PlacesOfResidenceId")]
        public PlacesOfResidence PlacesOfResidence { get; set; }
        public int UtilitiesId { get; set; }
        [ForeignKey("UtilitiesId")]
        public Utilities Utilities { get; set; }
        public string Indicator { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public bool Paid { get; set; }
    }
}
