using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Models
{
    public class Departments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<StatementsTypes> StatementsTypes { get; set; }
    }
}
