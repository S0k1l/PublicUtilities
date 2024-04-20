using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Repository
{
    public class IndicatorsRepository : IIndicatorsRepository
    {
        private readonly AppDbContext _context;

        public IndicatorsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<IndicatorsViewModel>> GetIndeicatorsByUserName(string userName)
        {

            var placesOfResidence = await _context.UsersPlacesOfResidence
                .Where(upor => upor.AppUser.UserName == userName)
                .Select(upor => new
                {
                    upor.PlacesOfResidenceId,
                    upor.PlacesOfResidence.Streets.Name,
                    upor.PlacesOfResidence.House,
                    upor.PlacesOfResidence.Apartment
                })
                .ToListAsync();

            var utilitiesTypes = new List<Utilities> { 
                await GetUtilities("Вода"),
                await GetUtilities("Газ"),
                await GetUtilities("Електроенергія"),
            };

            var indicators = new List<IndicatorsViewModel>();

            foreach (var item in placesOfResidence)
            {
                var userIndicators = new List<UserIndicators>();
                foreach (var utilityType in utilitiesTypes)
                {
                    var newIndicator = await _context.Indicators
                        .Where(i => i.PlacesOfResidenceId == item.PlacesOfResidenceId && i.Utilities.Name == utilityType.Name)
                        .OrderByDescending(i => i.Date)
                        .Select(i => new UserIndicators
                        {
                            Id = i.Id,
                            UtilitiesId = i.Utilities.Id,
                            UtilitiesType = i.Utilities.Name,
                            Indicator = i.Indicator,
                        })
                        .FirstOrDefaultAsync();
                    userIndicators.Add(newIndicator);
                }
                indicators.Add(new IndicatorsViewModel
                {
                    PlacesOfResidenceId = item.PlacesOfResidenceId,
                    PlacesOfResidence = $"Вул. {item.Name}, буд. {item.House}, кв. {item.Apartment}",
                    UserIndicators = userIndicators.OrderBy(m => m.UtilitiesType).ToList()
                });
            }
            return indicators;
        }

        public async Task<Utilities> GetUtilities(string utilitiesName)
        {
            return await _context.Utilities
                                    .Where(u => u.Name == utilitiesName)
                                    .OrderByDescending(u => u.Date)
                                    .FirstOrDefaultAsync();
        }

        public bool AddIndicator(Indicators indicators)
        {
            _context.Add(indicators);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
