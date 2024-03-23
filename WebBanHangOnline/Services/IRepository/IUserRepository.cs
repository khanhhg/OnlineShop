using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Services.IRepository
{
    public interface IUserRepository
    {
        Task<UserProfile> Get(string UserId);
        Task Add(UserProfile userProfile);
        Task<UserProfile> Update(UserProfile userProfile);
        Task Delete(UserProfile userProfile);

    }
}
