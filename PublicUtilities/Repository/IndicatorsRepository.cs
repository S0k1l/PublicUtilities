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

        public async Task<ICollection<IndicatorsListViewModel>> GetThoseWhoDontUploadIndicators()
        {
            var currYear = DateTime.Now.Year;
            var prevMonth = DateTime.Now.Month -1;

            DateTime startDate = new DateTime(currYear, prevMonth, DateTime.DaysInMonth(currYear, prevMonth) - 2);
                        
            var porsId = await _context.PlacesOfResidence.Select(por => por.Id).ToListAsync();

            var model = new List<IndicatorsListViewModel>();

            foreach (var porId in porsId)
            {
                var asd = await _context.Indicators
                    .Where(i => i.PlacesOfResidenceId == porId)
                    .GroupBy(i => i.UtilitiesId)
                    .Select(g => new
                    {
                        PlacesOfResidenceId = g.Key,
                        LatestUploadDate = g.Max(i => i.Date),
                        UtilitiesName = g.Select(g => g.Utilities.Name).FirstOrDefault(),
                        Street = g.Select(g => g.PlacesOfResidence.Streets.Name).FirstOrDefault(),
                        House = g.Select(g => g.PlacesOfResidence.House).FirstOrDefault(),
                        Apartment = g.Select(g => g.PlacesOfResidence.Apartment).FirstOrDefault(),
                    })
                    .Where(i => i.LatestUploadDate < startDate)
                    .Select(i => new IndicatorsListViewModel
                    {
                        PlacesOfResidenceId = i.PlacesOfResidenceId,
                        UtilitiesName = i.UtilitiesName,
                        PlacesOfResidence = $"Вул. {i.Street}, буд. {i.House}, кв. {i.Apartment}",
                        LastUploaded = i.LatestUploadDate.ToShortDateString()
                    })
                    .ToListAsync();

                model.AddRange(asd);
            }

            return model;
        }

        public async Task<PlacesOfResidence> GetPlacesOfResidence(int id)
        {
            return await _context.PlacesOfResidence.FirstOrDefaultAsync(por => por.Id == id);
        }
    }
}
