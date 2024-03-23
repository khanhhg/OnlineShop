using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Services.IRepository
{
    public interface INewsRepository
    {
        Task<IList<News>> GetAll();
        Task<IList<News>> GetNewHome();
        Task<News> Get(int newsId);
        Task Add(News news);
        Task<News> Update(News news);
        Task Delete(News news);

        Task IsActive(News news);
    }
}
