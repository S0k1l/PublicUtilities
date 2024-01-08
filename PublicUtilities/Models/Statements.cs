using PublicUtilities.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicUtilities.Models
{
    public class Statements
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Departments Department { get; set; }
        public int StatementsTypeId { get; set; }
        [ForeignKey("StatementsTypeId")]
        public StatementsTypes StatementsType { get; set; }
        public string Text { get; set; }
        public string StatementUrl { get; set; }
        public StatementsStatus Status { get; set; }
        public DateTime Date { get; set; }
        public ICollection<UsersStatements> UsersStatements { get; set; }
    }
}
