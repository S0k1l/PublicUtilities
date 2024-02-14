using Microsoft.AspNetCore.Identity;

namespace PublicUtilities.Models
{
    public class AppUser : IdentityUser
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<UsersStatements> UsersStatementsId { get; set; }
        public ICollection<UsersPlacesOfResidence> UsersPlacesOfResidenceId { get; set; }
    }
}
