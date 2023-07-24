using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Data.IRepository
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
