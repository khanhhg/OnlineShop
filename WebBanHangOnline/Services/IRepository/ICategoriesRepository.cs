using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Services.IRepository
{
    public interface ICategoriesRepository
    {
        Task<IList<Category>> GetAll();
        Task<Category> Get(int CategoryId);
        Task Add(Category category);
        Task<Category> Update(Category category);
        Task Delete(Category category);
    }
}
