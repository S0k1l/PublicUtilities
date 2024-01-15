using PublicUtilities.Models;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.ViewModels
{
    public class IndicatorsViewModel
    {
        public int PlacesOfResidenceId { get; set; }
        public string PlacesOfResidence { get; set; }
        public ICollection<UserIndicators> UserIndicators { get; set; }
        public IndicatorsViewModel()
        {
            UserIndicators = new List<UserIndicators>();
        }
    }

    public class UserIndicators
    {
        public int Id { get; set; }
        public int UtilitiesId { get; set; }
        public string UtilitiesType { get; set; }
        public string Indicator { get; set; }
    }


}
