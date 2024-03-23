using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Services.Repository
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Category>> GetAll()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category> Get(int categoryId)
        {
            return await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == categoryId);
        }

        public async Task<Category> Update(Category categoryChanges)
        {
            var categor = _context.Category.Attach(categoryChanges);
            categor.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return categoryChanges;
        }
    }
}
