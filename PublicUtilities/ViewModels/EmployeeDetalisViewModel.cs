using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class EmployeeDetalisViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Departement { get; set; }
    }
}
