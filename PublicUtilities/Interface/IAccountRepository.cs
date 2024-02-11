using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface IAccountRepository
    {
        Task<PlacesOfResidence> GetPlacesOfResidencer(string street, string house, string apartment);
        Task<UsersPlacesOfResidence> GetUserPlacesOfResidence(string userId, int placesOfResidenceId);
        Task<ICollection<PlacesOfResidenceViewModel>> GetUserPlacesOfResidencerById(string id);
        Task<AppUser> GetUserByUserName(string userName);
        bool AddPlaceOfResidence(UsersPlacesOfResidence usersPlacesOfResidence);
        bool DeleteUserPlaceOfResidence(UsersPlacesOfResidence usersPlacesOfResidence);
        bool UpdateUserInfo(AppUser user);
        bool Save();
    }
}
