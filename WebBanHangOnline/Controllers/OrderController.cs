using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanHangOnline.Services.IRepository;
using X.PagedList;

namespace WebBanHangOnline.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        // GET: Admin/Order
        public async Task<ActionResult> Index(string Searchtext, int? page = 1)
        {
            string vUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _orderRepository.GetOderByUser(vUserID);

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

        public async Task<ActionResult> ViewOrder(int id)
        {
            var item = await _orderRepository.Get(id);
            return View(item);
        }
        public async Task<ActionResult> Partial_Products(int id)
        {
            var items = await _orderRepository.GetOrderDetail(id);
            return PartialView("_Partial_Products", items);
        }     
    }
}
