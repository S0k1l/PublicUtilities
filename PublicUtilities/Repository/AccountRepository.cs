using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool AddPlaceOfResidence(UsersPlacesOfResidence usersPlacesOfResidence)
        {
            _context.Add(usersPlacesOfResidence);
            return Save();
        }

        public bool DeleteUserPlaceOfResidence(UsersPlacesOfResidence usersPlacesOfResidence)
        {
            _context.Remove(usersPlacesOfResidence);
            return Save();
        }

        public Task<PlacesOfResidence> GetPlacesOfResidencer(string street, string house, string apartment)
        {
            return _context.PlacesOfResidence
                .FirstOrDefaultAsync(por => por.Streets.Name == street &&
                por.House == house &&
                por.Apartment == apartment);
        }

        public async Task<AppUser> GetUserByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<UsersPlacesOfResidence> GetUserPlacesOfResidence(string userId, int placesOfResidenceId)
        {
            return await _context.UsersPlacesOfResidence
                .FirstOrDefaultAsync(upor => upor.AppUserId == userId && upor.PlacesOfResidenceId == placesOfResidenceId);
        }

        public async Task<ICollection<PlacesOfResidenceViewModel>> GetUserPlacesOfResidencerById(string id)
        {
            return await _context.UsersPlacesOfResidence
                .Where(upor => upor.AppUserId == id)
                .Select(u => new PlacesOfResidenceViewModel
                {
                    Id = u.PlacesOfResidence.Id,
                    Street = u.PlacesOfResidence.Streets.Name,
                    Building = u.PlacesOfResidence.House,
                    Apartment = u.PlacesOfResidence.Apartment,
                })
                .ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUserInfo(AppUser user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
