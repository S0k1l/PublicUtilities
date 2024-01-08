using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Models
{
    public class Departments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Statements> Statements { get; set; }
    }
}
