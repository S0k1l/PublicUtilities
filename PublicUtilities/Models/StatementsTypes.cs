using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Models
{
    public class StatementsTypes
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int SignatureCount { get; set; }
        public virtual ICollection<Statements> Statements { get; set; }
    }
}
