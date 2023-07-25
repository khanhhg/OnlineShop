using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Data.IRepository
{
    public interface IUserRepository
    {
        Task<UserProfile> Get(string UserId);
        Task Add(UserProfile userProfile);
        Task<UserProfile> Update(UserProfile userProfile);
        Task Delete(UserProfile userProfile);

    }
}
