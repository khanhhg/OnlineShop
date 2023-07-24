using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Models
{
	public class HomeViewModel
	{
		public IList<Product> ProductSales { get; set; }
		public IList<Product> Product_ByCategory { get; set; }
        public IList<ProductCategory> CateroryArrivals { get; set; }
    }
}
