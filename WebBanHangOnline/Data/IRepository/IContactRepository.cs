using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Data.IRepository
{
    public interface IContactRepository
    {
        Task<IList<Contact>> GetAll();
        Task<Contact> Get(int contactId);
        Task Add(Contact contact);
        Task<Contact> Update(Contact contact);
        Task Delete(Contact contact);
        Task IsRead(Contact contact);
    }
}
