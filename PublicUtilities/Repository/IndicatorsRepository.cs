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

        //TODO needs rework
        public async Task<ICollection<IndicatorsViewModel>> GetIndeicatorsByUserName(string userName)
        {

            var placesOfResidence = await _context.PlacesOfResidence
                .Join(_context.UsersPlacesOfResidence, pr => pr.Id, upr => upr.PlacesOfResidenceId, (pr, upr) => new { pr, upr })
                .Join(_context.Streets, x => x.pr.StreetsId, s => s.Id, (x, s) => new { x.pr, x.upr, s })
                .Where(x => x.upr.AppUser.UserName == userName)
                .Select(x => new
                {
                    x.pr.Id,
                    x.s.Name,
                    x.pr.House,
                    x.pr.Apartment
                })
                .ToListAsync();

            var utilitiesTypes = await _context.Utilities.ToListAsync();
            var indicators = new List<IndicatorsViewModel>();

            foreach (var item in placesOfResidence)
            {
                var userIndicators = new List<UserIndicators>();
                foreach (var utilityType in utilitiesTypes)
                {
                    var newIndicator = await _context.Indicators
                        .Join(_context.Utilities, i => i.UtilitiesId, u => u.Id, (i, u) => new { i, u })
                        .Where(x => x.i.PlacesOfResidenceId == item.Id && x.u.Name == utilityType.Name && x.i.isPaid == false)
                        .OrderByDescending(x => x.i.Date)
                        .Select(x => new UserIndicators
                        {
                            Id = x.i.Id,
                            UtilitiesId = x.u.Id,
                            UtilitiesType = x.u.Name,
                            Indicator = x.i.Indicator,
                        })
                        .FirstOrDefaultAsync();
                    userIndicators.Add(newIndicator);
                }
                indicators.Add(new IndicatorsViewModel
                {
                    PlacesOfResidenceId = item.Id,
                    PlacesOfResidence = $"Вул. {item.Name}, буд. {item.House}, кв. {item.Apartment}",
                    UserIndicators = userIndicators.OrderBy(m => m.UtilitiesType).ToList()
                });
            }
            return indicators;
        }

        public async Task<decimal> GetUtilitiesPriceById(int utilitiesId)
        {
            return await _context.Utilities
                .Where(u => u.Id == utilitiesId)
                .Select(u => u.Price).FirstOrDefaultAsync();
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
