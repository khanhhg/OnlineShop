using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.ViewComponents
{
    public class SubscribeViewComponent : ViewComponent
    {        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = new Subscribe();
            return View(item);
        }
    }
}
