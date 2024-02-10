using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;

namespace PublicUtilities.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly AppDbContext _context;

        public HomeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<News>> GetTop4News()
        {
            return await _context.News
                .OrderByDescending(n => n.Date)
                .Take(4)
                .ToListAsync();
        }
    }
}
