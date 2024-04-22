using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class CreateNotificationViewModel
    {
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Apartment { get; set; }

        [Required(ErrorMessage = "Заповніть це поле")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Заповніть це поле")]
        public string Text { get; set; }
    }
}
