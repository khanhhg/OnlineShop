using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Models
{
	public class HomeViewModel
	{
		public List<Product> ProductSales { get; set; }
		public List<Product> Product_ByCategory { get; set; }
        public List<ProductCategory> CateroryArrivals { get; set; }
    }
}
