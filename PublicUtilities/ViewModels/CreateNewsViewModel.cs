using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class CreateNewsViewModel
    {
        [Required(ErrorMessage = "Заповніть це поле")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Заповніть це поле")]
        public string Text { get; set; }
        public IFormFile? Image { get; set; }
    }
}
