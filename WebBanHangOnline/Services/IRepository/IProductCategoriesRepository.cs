using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Services.IRepository
{
    public interface IProductCategoriesRepository
    {
        Task<IList<ProductCategory>> GetAll();
        Task<ProductCategory> Get(int Id);
        Task Add(ProductCategory Productcategory);
        Task<ProductCategory> Update(ProductCategory Productcategory);
        Task<bool> Delete(ProductCategory Productcategory);
    }
}
