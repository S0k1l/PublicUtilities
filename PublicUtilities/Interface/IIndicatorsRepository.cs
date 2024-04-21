using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface IIndicatorsRepository
    {
        Task<ICollection<IndicatorsViewModel>> GetIndeicatorsByUserName(string userName);
        Task<Utilities> GetUtilities(string utilitiesName);
        Task<ICollection<IndicatorsListViewModel>> GetThoseWhoDontUploadIndicators();
        Task<PlacesOfResidence> GetPlacesOfResidence(int id);
        bool AddIndicator(Indicators indicators);
        bool Save();

    }
}
