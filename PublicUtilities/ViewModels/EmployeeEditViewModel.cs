using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class EmployeeEditViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Електрона пошта\"")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Номер телефону\"")]
        [MaxLength(10)]
        [Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Прізвище\"")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"Ім'я\"")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Будь ласка, заповніть поле \"По батькові\"")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Будь ласка, виберіть департамент")]
        public string Departement { get; set; }
    }
}
