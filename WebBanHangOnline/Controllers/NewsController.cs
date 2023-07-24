using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.IRepository;
using WebBanHangOnline.Models.EF;
using X.PagedList;

namespace WebBanHangOnline.Controllers
{
    public class NewsController : Controller
    {
        INewsRepository _INews;
        public NewsController(INewsRepository iNewsRepository)
        {
            _INews = iNewsRepository;
        }
        public async Task<ActionResult> Index(int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = await _INews.GetAll();
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public async Task<ActionResult> Detail(int id)
        {
            var item = await _INews.Get(id);
            return View(item);
        }
        public async Task<ActionResult> Partial_News_Home()
        {
            var items = await _INews.GetNewHome();        
			return PartialView("_Partial_News_Home", items);
		}
    }
}
