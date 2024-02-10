using PublicUtilities.Models;

namespace PublicUtilities.Interface
{
    public interface IHomeRepository
    {
        Task<ICollection<News>> GetTop4News();
    }
}
