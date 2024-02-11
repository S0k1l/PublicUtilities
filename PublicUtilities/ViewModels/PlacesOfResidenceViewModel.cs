using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class PlacesOfResidenceViewModel
    {
        public int Id { get; set; }
        public string Street { get; set; }

        public string Building { get; set; }

        public string Apartment { get; set; }
    }
}
