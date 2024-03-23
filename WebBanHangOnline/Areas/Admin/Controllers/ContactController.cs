using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ContactController : Controller
    {        
        IContactRepository _IContact;
        public ContactController(IContactRepository contactRepository)
        {
            _IContact = contactRepository;        
        }
        // GET: Admin/Categories
        public async Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            IEnumerable<Contact> items = await _IContact.GetAll();  
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Email.Contains(Searchtext) || x.Message.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _IContact.Get(id);
            if (item != null)
            {
                await _IContact.Delete(item);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public async Task<ActionResult> Detail(int id)
        {
            var item = await _IContact.Get(id);
            if (item != null)
            {
                await _IContact.IsRead(item);
            }
            return View(item);
        }
    }
}
