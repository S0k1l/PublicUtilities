using PublicUtilities.Data.Enum;
using PublicUtilities.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicUtilities.ViewModels
{
    public class MyStatementsViewModel
    {
        public ICollection<MyStatements> MyStatements { get; set; }
        public ICollection<MyStatements> OthersStatements { get; set; }
    }

    public class MyStatements
    {
        public int Id { get; set; }
        public string StatementsType { get; set; }
        public StatementsStatus Status { get; set; }
        public DateTime Date { get; set; }
    }
}
