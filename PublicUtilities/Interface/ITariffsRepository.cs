using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface ITariffsRepository
    {
        Task<decimal> GetTariffPriceAsync(string UtilitiesName);
        bool Add(decimal UtilitiesPrice, string UtilitiesName);
        bool Save();
    }
}
