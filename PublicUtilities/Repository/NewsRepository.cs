using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly AppDbContext _context;

        public NewsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<NewsViewModel> GetAllNews(int page)
        {
            int pageSize = 5;
            var data = await _context.News.ToListAsync();
            var totalCount = data.Count();
            var items = data
                .OrderByDescending(d => d.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var viewModel = new NewsViewModel
            {
                Items = items,
                PageNumber = page,
                TotalPages = totalPages
            };
            return viewModel;
        }

        public async Task<News> GetNewsById(int id)
        {
            return await _context.News.FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}
