using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using WebBanHangOnline.Data;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Admin/Order
        public ActionResult Index(string Searchtext, int? page = 1)
        {
            var items = _context.Order.OrderByDescending(x => x.CreatedDate).ToList();

            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Code.Contains(Searchtext) || x.CustomerName.Contains(Searchtext)).ToList();
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ViewOrder (int id)
        {
            var item = _context.Order.Where(x=>x.OrderId ==id).Include(x=>x.OrderDetails).FirstOrDefault();
            return View(item);
        }

        public ActionResult Partial_Products(int id)
        {
            var items = _context.OrderDetail.Where(x => x.OrderId == id).Include(x=>x.Product).ToList();
            return PartialView("_Partial_Products", items);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = _context.Order.Find(id);
            if (item != null)
            {
                _context.Order.Attach(item);
                item.TypePayment = trangthai;
                _context.Entry(item).Property(x => x.TypePayment).IsModified = true;
                _context.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }    
    }
}
