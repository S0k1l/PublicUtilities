using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class RegisterViewModel
    {
        
        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Електрона пошта\"")]
        [EmailAddress]
        public string Email { get; set; }
       
        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Номер телефону\"")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Пароль\"")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Повторіть пароль\"")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Прізвище\"")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Ім'я\"")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"По батькові\"")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Вулиця проживання\"")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Будинок проживання\"")]
        public string Building { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Квартира проживання\"")]
        public string Apartment { get; set; }
    }
}
