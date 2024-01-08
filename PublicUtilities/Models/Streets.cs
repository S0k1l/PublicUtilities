using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Models
{
    public class Streets
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PlacesOfResidence> PlacesOfResidence { get; set; }
    }
}
