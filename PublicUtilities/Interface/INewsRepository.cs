using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface INewsRepository
    {
        Task<NewsViewModel> GetAllNews(int page);
        Task<News> GetNewsById(int id);
        bool Add(News news);
        bool Update(News news);
        bool Delete(News news);
        bool Save();
    }
}
