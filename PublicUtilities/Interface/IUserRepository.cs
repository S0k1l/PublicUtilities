using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(string id);

        bool Delete(AppUser user);
        bool Update(AppUser user);
        bool Save();
    }
}
