using WebBanHangOnline.Data.Models.Dtos;
using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Services.IRepository
{
    public interface IProductCategoriesRepository
    {
        Task<IList<ProductCategory>> GetAll();
        Task<ProductCategory> Get(int Id);
        Task <ProductCategory> Add(ProductCategoryDto Productcategory);
        Task<ProductCategory> Update(ProductCategoryDto Productcategory);
        Task<bool> Delete(ProductCategory Productcategory);
    }
}
