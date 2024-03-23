using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Services.Repository
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Comments comments)
        {
            _context.Add(comments);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Comments comments)
        {
            _context.Comments.Remove(comments);
            await _context.SaveChangesAsync();
        }
        public async Task<IList<Comments>> GetAll()
        {
            return await _context.Comments.ToListAsync();
        }
        public async Task<IList<Comments>> GetByProductID(int Id)
        {
            return await _context.Comments.Where(x => x.ProductId == Id).Include(x => x.Product).ToListAsync();
        }
        public async Task<Comments> Get(int Id)
        {
            return await _context.Comments.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<Comments> Update(Comments commentsChanges)
        {
            var comments = _context.Comments.Attach(commentsChanges);
            comments.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return commentsChanges;
        }
    }
}
