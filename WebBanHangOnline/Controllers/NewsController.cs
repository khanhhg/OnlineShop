using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;
using X.PagedList;

namespace WebBanHangOnline.Controllers
{
    public class NewsController : Controller
    {
        INewsRepository _newsRepository;
        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<ActionResult> Index(int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = await _newsRepository.GetAll();
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public async Task<ActionResult> Detail(int id)
        {
            var item = await _newsRepository.Get(id);
            return View(item);
        }      
    }
}
