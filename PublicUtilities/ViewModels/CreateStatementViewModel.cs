using PublicUtilities.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class CreateStatementViewModel
    {
        public int StatementId { get; set; }
        public string StatementType { get; set; }
        public string? Street { get; set; }
        public string Text { get; set; }
        public IFormFile? Image { get; set; }
        public bool isPhotoNeeded { get; set; }
        public bool isStreetNeeded { get; set; }
        public int SignatureCount { get; set; }
    }
}
