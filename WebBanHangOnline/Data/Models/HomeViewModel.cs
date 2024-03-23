using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Data.Models
{
    public class HomeViewModel
    {
        public IList<Product> ProductSales { get; set; }
        public IList<Product> Product_ByCategory { get; set; }
        public IList<ProductCategory> CateroryArrivals { get; set; }
    }
}
