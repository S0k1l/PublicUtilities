using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicUtilities.Models
{
    public class StatementsTypes
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int SignatureCount { get; set; }
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Departments Department { get; set; }
        public bool isPhotoNeeded { get; set; }
        public bool isStreetNeeded { get; set; }
        public virtual ICollection<Statements> Statements { get; set; }
    }
}
