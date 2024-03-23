using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.ViewComponents
{
    public class CreateCommentViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = new Comments();
            return View(item);
        }
    }
}
