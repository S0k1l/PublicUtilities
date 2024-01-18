using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface IStatisticRepository
    {
        Task<ICollection<StatisticViewModel>> GetUserStatisticByUtilitiesType(string userName, string utylitiesType);
    }
}
