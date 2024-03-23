using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.ViewComponents
{
    public class ProductCategoryViewComponent : ViewComponent
	{
		IProductCategoriesRepository _productCategoriesRepository;
		public ProductCategoryViewComponent(IProductCategoriesRepository productCategoriesRepository)
		{
			_productCategoriesRepository = productCategoriesRepository;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var items = await _productCategoriesRepository.GetAll();

			return View(items);
		}
	}
}
