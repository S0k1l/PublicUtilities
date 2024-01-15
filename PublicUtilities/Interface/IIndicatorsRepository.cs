using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface IIndicatorsRepository
    {
        Task<ICollection<IndicatorsViewModel>> GetIndeicatorsByUserName(string userName);
        Task<decimal> GetUtilitiesPriceById(int utilitiesId);
        bool AddIndicator(Indicators indicators);
        bool Save();

    }
}
