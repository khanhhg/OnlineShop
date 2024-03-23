using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.ViewComponents
{
    public class ListCommentViewComponent : ViewComponent
    {

        ICommentsRepository _commentsRepository;
        public ListCommentViewComponent(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var items = await _commentsRepository.GetByProductID(id);         
            return View(items);
        }
    }
}
