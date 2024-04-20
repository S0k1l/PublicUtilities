using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface IIndicatorsRepository
    {
        Task<ICollection<IndicatorsViewModel>> GetIndeicatorsByUserName(string userName);
        Task<Utilities> GetUtilities(string utilitiesName);
        bool AddIndicator(Indicators indicators);
        bool Save();

    }
}
