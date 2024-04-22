using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicUtilities.Models
{
    public class PlacesOfResidence
    {
        public int Id { get; set; }
        public int StreetsId { get; set; }
        [ForeignKey("StreetsId")]
        public Streets Streets { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public ICollection<UsersPlacesOfResidence> UsersPlacesOfResidence { get; set; }
        public ICollection<Indicators> Indicators { get; set; }
    }
}
