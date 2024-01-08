using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Models
{
    public class UsersStatements
    {
        public string AppUserId { get; set; }
        public int StatementsId { get; set; }
        public AppUser AppUser { get; set; }
        public Statements Statements { get; set; }

    }
}
