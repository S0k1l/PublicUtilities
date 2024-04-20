using PublicUtilities.Models;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class TariffsViewModel
    {
        [Required(ErrorMessage = "Заповніть це поле")]
        public decimal WaterPrice { get; set; }

        [Required(ErrorMessage = "Заповніть це поле")]
        public decimal GasPrice { get; set; }

        [Required(ErrorMessage = "Заповніть це поле")]
        public decimal ElectricityPrice { get; set; }
    }
}
