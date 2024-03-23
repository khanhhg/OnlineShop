using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Services.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;
        public ContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Contact contact)
        {
            _context.Add(contact);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Contact contact)
        {
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
        }
        public async Task<IList<Contact>> GetAll()
        {
            return await _context.Contact.ToListAsync();
        }
        public async Task<Contact> Get(int contactId)
        {
            return await _context.Contact.FirstOrDefaultAsync(x => x.ContactId == contactId);
        }
        public async Task<Contact> Update(Contact contactChanges)
        {
            var contact = _context.Contact.Attach(contactChanges);
            contact.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return contactChanges;
        }
        public async Task IsRead(Contact contact)
        {
            contact.IsRead = true;
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
