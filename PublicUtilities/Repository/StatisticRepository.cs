using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Repository
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly AppDbContext _context;

        public StatisticRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<StatisticViewModel>> GetUserStatisticByUtilitiesType(string userName, string utylitiesType)
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
            var indicators = new List<StatisticViewModel>();

            foreach (var item in placesOfResidence)
            {
                var newIndicator = await _context.Indicators
                    .Where(x => x.PlacesOfResidenceId == item.Id && x.Utilities.Name == utylitiesType)
                    .OrderBy(x => x.Date)
                    .Select(x => new DataPoint
                    {
                        Indicator = x.Indicator,
                        Date = x.Date.ToString("Y"),
                        Price = x.Price,
                    })
                    .Take(12)
                    .ToListAsync();
                
                indicators.Add(new StatisticViewModel
                {
                    PlacesOfResidence = $"вул. {item.Name}, буд. {item.House}, кв. {item.Apartment}",
                    DataPoints = newIndicator,
                });
            }

            return indicators;
        }
    }
}
