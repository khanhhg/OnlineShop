using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Controllers
{
    public class CommentsController : Controller
    {
        private readonly INotyfService _toastNotification;
        ICommentsRepository _commentsRepository;
        public CommentsController(ICommentsRepository commentsRepository, INotyfService toastNotification)
        {
            _commentsRepository = commentsRepository;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comments comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedDate = DateTime.Now;              
                await _commentsRepository.Add(comment);
                _toastNotification.Success("Send message success");              
            }
            else
            {
                _toastNotification.Error("Send message error");
               
            }
            return LocalRedirect("/products/detail?id=" + comment.ProductId);
        }
    }
}
