﻿using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Data.IRepository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
        }   
        public async Task Delete(Product product)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
        }     
        public async Task<IList<Product>> GetAll()
        {
            return await _context.Product.Include(x=>x.ProductCategory).Include(x=>x.ProductImages).OrderByDescending(x=>x.ProductId).ToListAsync();
        }

        public async Task<Product> Get(int Id)
        {
            return await _context.Product.Include(x => x.ProductCategory).Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.ProductId == Id);
        }

        public async Task<Product> Update(Product productChanges)
        {
            var product = _context.Product.Attach(productChanges);
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return productChanges;
        }

        public async Task IsActive(Product product)
        {
            product.IsActive = !product.IsActive;
            _context.Entry(product).State = EntityState.Modified;          
            await _context.SaveChangesAsync();
        }
        public async Task IsSale(Product product)
        {
            product.IsSale = !product.IsSale;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task IsHome(Product product)
        {
            product.IsHome = !product.IsHome;
            _context.Entry(product).State = EntityState.Modified; 
            await _context.SaveChangesAsync();
        }

        // Product Image
        public async Task AddImage(ProductImage productImage)
        {
            _context.Add(productImage);
            await _context.SaveChangesAsync();
        }
        public async Task<ProductImage> UpdateImage(ProductImage productImageChanges)
        {
            var productImage = _context.ProductImage.Attach(productImageChanges);
            productImage.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return productImageChanges;
        }
        public async Task DeleteImage(ProductImage productImage)
        {
            _context.ProductImage.Remove(productImage);
            await _context.SaveChangesAsync();
        }
        public async Task<IList<ProductImage>> GetImageByProduct(int Id)
        {
            return await _context.ProductImage.Where(x=>x.ProductId == Id).OrderByDescending(x=>x.ProductImageId).ToListAsync();
        }
        public async Task<ProductImage> GetImage(int Id)
        {
            return await _context.ProductImage.FirstOrDefaultAsync(x => x.ProductImageId == Id);
        }
        public async Task IsDefault(ProductImage productImage)
        {
            productImage.IsDefault = !productImage.IsDefault;
            _context.Entry(productImage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Product> Update_ViewCount(Product productChanges)
        {
            var product = _context.Product.Attach(productChanges);
            if (productChanges.ViewCount ==null)
            {
                productChanges.ViewCount = 1;
            }
            else
            {
                productChanges.ViewCount = productChanges.ViewCount + 1;
            }         
            _context.Entry(productChanges).Property(x => x.ViewCount).IsModified = true;        
            await _context.SaveChangesAsync();
            return productChanges;
        }
        public async Task<double> avgRate(int Id)
        {
            double avg = 0;
            var obj = await _context.Comments.Where(x => x.ProductId == Id).ToListAsync();
            if (obj.Count > 0)
            {
                avg = (double)obj.Select(x => x.Rate).Average();
            }         
            return avg;
        }
        public async Task<IList<ProductCategory>> GetAllProductCategory()
        {
            return await _context.ProductCategory.ToListAsync();
        }
        public async Task<IList<Product>> GetViewProducts(int Id, int Top, bool IsHome, bool IsSale)
        {
            IList<Product> items;          

            if (Top > 0) // Get Top
            {
                items = await _context.Product.Where(x => (Id == 0 || x.ProductCategoryId == Id)
                                           && (IsHome == false || x.IsHome == IsHome)
                                           && (IsSale == false || x.IsSale == IsSale)
                                           && (x.IsActive == true)
                                           && (x.Quantity > 0)
                                      ).Include(x => x.ProductCategory).Include(x => x.ProductImages).OrderByDescending(x => x.ProductId).Take(Top).ToListAsync();
            }
            else // Get All
            {
                items = await _context.Product.Where(x => (Id == 0 || x.ProductCategoryId == Id)
                                          && (IsHome == false || x.IsHome == IsHome)
                                          && (IsSale == false || x.IsSale == IsSale)
                                          && (x.IsActive == true)
                                          && (x.Quantity > 0)
                                     ).Include(x => x.ProductCategory).Include(x => x.ProductImages).OrderByDescending(x => x.ProductId).ToListAsync();
            }
            return items;
        }
    }
}
