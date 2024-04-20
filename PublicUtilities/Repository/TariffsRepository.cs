using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Repository
{
    public class TariffsRepository : ITariffsRepository
    {
        private readonly AppDbContext _context;

        public TariffsRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(decimal UtilitiesPrice, string UtilitiesName)
        {
            var utilities = new Utilities
            {
                Name = UtilitiesName,
                Price = UtilitiesPrice,
                Date = DateTime.Now,
            };

            _context.Add(utilities);
            return Save();
        }

        public async Task<decimal> GetTariffPriceAsync(string UtilitiesName)
        {
            return await _context.Utilities
                                    .Where(u => u.Name == UtilitiesName)
                                    .OrderByDescending(u => u.Date)
                                    .Select(u => u.Price)
                                    .FirstOrDefaultAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
