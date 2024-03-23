using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.ViewComponents
{
    public class NewsViewComponent : ViewComponent
    {
        INewsRepository _newsRepository;
        public NewsViewComponent(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _newsRepository.GetNewHome();
           
            return View(items);
        }
    }
}
