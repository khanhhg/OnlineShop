using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.Models.Dtos;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Services.Repository
{
    public class ProductCategoriesRepository : IProductCategoriesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductCategoriesRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductCategory> Add(ProductCategoryDto productCategoryDto)
        {
            var productCategory = _mapper.Map<ProductCategory>(productCategoryDto);
            _context.ProductCategory.Add(productCategory);
            await _context.SaveChangesAsync();

            return productCategory;
        }

        public async Task<bool> Delete(ProductCategory productCategory)
        {
            var productItem = _context.Product.Where(x => x.ProductCategoryId == productCategory.ProductCategoryId).ToList();
            if (productItem.Count > 0)
            {
                _context.ProductCategory.Remove(productCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IList<ProductCategory>> GetAll()
        {
            return await _context.ProductCategory.ToListAsync();
        }

        public async Task<ProductCategory> Get(int Id)
        {
            return await _context.ProductCategory.FirstOrDefaultAsync(x => x.ProductCategoryId == Id);
        }

        public async Task<ProductCategory> Update(ProductCategoryDto productCategoryDto)
        {
            var productCategory = _mapper.Map<ProductCategory>(productCategoryDto);
            var productCategoryChanges = _context.ProductCategory.Attach(productCategory);
            productCategoryChanges.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return productCategory;
        }
    }
}
