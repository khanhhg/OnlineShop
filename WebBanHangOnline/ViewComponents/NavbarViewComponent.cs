using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Linq;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        ICategoriesRepository _categoriesRepository;
        public NavbarViewComponent(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categoriesRepository.GetAll();
            items = items.OrderBy(x => x.Position).ToList();
            return View(items);
        }
    }
}
