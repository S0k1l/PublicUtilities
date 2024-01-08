using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Models
{
    public class UsersPlacesOfResidence
    {
        public string AppUserId { get; set; }
        public int PlacesOfResidenceId { get; set; }
        public AppUser AppUser { get; set; }
        public PlacesOfResidence PlacesOfResidence { get; set; }
    }
}
