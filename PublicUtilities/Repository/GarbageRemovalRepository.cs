using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;

namespace PublicUtilities.Repository
{
    public class GarbageRemovalRepository : IGarbageRemovalRepository
    {
        private readonly AppDbContext _context;

        public GarbageRemovalRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<GarbageRemoval> GetGarbageRemovalAsync()
        {
            return await _context.GarbageRemoval.FirstOrDefaultAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(GarbageRemoval garbageRemoval)
        {
            _context.Update(garbageRemoval);
            return Save();
        }
    }
}
