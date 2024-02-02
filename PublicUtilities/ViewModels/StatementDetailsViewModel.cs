using PublicUtilities.Data.Enum;
using PublicUtilities.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicUtilities.ViewModels
{
    public class StatementDetailsViewModel
    {
        public int Id { get; set; }
        public string StatementsType { get; set; }
        public string? Street { get; set; }
        public string Text { get; set; }
        public string? StatementUrl { get; set; }
        public StatementsStatus Status { get; set; }
        public string Date { get; set; }
    }
}
