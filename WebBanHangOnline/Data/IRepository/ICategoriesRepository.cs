using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Data.IRepository
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
