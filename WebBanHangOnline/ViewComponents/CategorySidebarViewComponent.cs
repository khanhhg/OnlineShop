using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.ViewComponents
{
    public class CategorySidebarViewComponent : ViewComponent
	{
		IProductCategoriesRepository _productCategoriesRepository;
		public CategorySidebarViewComponent(IProductCategoriesRepository productCategoriesRepository)
		{
            _productCategoriesRepository = productCategoriesRepository;
		}
		public async Task<IViewComponentResult> InvokeAsync(int? id)
		{
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            else
            {
                ViewBag.CateId = 0;
            }
            var items = await _productCategoriesRepository.GetAll();
            return View(items);
		}
	}
}
