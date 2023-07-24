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
        Task<IList<Product>> GetProduts_By_Caterory(int Id,int Top);
        Task<IList<Product>> GetProduts_Promotion(int Id,int Top);
        Task<Product> Update_ViewCount(Product product);
      
    }
}
