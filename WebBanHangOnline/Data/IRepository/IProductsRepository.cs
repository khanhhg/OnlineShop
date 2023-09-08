using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Data.IRepository
{
    public interface IProductsRepository
    {
        Task<IList<Product>> GetAll();
        Task<Product> Get(int Id);
        Task Add(Product product);
        Task<Product> Update(Product product);
        Task Delete(Product product);

        Task IsActive(Product product);
        Task IsHome(Product product);
        Task IsSale(Product product);
   
        
        Task AddImage(ProductImage productImage);
        Task<ProductImage> UpdateImage(ProductImage productImage);
        Task DeleteImage(ProductImage productImage);
        Task<IList<ProductImage>> GetImageByProduct(int Id);
        Task<ProductImage> GetImage(int Id);
        Task IsDefault(ProductImage productImage);

        // Customer interface

        Task<Product> Update_ViewCount(Product product);
        Task<double> avgRate(int Id);
        Task<IList<ProductCategory>> GetAllProductCategory();
        Task<IList<Product>> GetViewProducts(int Id, int Top, bool IsHome, bool IsSale);
    }
}
