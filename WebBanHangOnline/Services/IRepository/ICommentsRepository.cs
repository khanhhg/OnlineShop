using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Services.IRepository
{
    public interface ICommentsRepository
    {
        Task<IList<Comments>> GetAll();
        Task<IList<Comments>> GetByProductID(int Id);
        Task<Comments> Get(int Id);
        Task Add(Comments comments);
        Task<Comments> Update(Comments comments);
        Task Delete(Comments comments);
    }
}
