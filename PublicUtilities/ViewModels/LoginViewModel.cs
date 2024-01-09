using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Електрона пошта\"")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Пароль\"")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
