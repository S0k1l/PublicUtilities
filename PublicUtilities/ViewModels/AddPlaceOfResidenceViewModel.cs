using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class AddPlaceOfResidenceViewModel
    {
        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Вулиця проживання\"")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Будинок проживання\"")]
        public string Building { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Квартира проживання\"")]
        public string Apartment { get; set; }
    }
}
