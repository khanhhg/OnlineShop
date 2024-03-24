using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebBanHangOnline.Services.IRepository;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class OrderController : Controller
    {
        IOrderRepository _order;

        public OrderController(IOrderRepository orderRepository)
        {
            _order = orderRepository;
        }
        // GET: Admin/Order
        public async Task<ActionResult> Index(string Searchtext, int? page = 1)
        {
            var items = await _order.GetAll();

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

        public async Task<ActionResult> ViewOrder (int id)
        {
            var  item = await _order.Get(id);
            return View(item);
        }
        public async Task<ActionResult> Partial_Products(int id)
        {
            var items = await _order.GetOrderDetail(id);
            return PartialView("_Partial_Products", items);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTT(int id, int status)
        {
            var item = await _order.Get(id);
            if (item != null)
            {
                await _order.ChangeStatus(item, status);
                return Json(new { message = "Success", Success = true });
            }
            else
            {
                return Json(new { message = "Unsuccess", Success = false });
            }       
        }    
    }
}
