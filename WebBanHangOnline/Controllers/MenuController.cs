using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
	public class MenuController : Controller
	{
		private readonly ApplicationDbContext _context;
		public MenuController(ApplicationDbContext context)
		{
			_context = context;
		}
		public ActionResult Index()
		{
			return View();
		}

		public IActionResult MenuTop()
		{
			var items = _context.Category.OrderBy(x => x.Position).ToList();
			return PartialView("_MenuTop", items);
		}

		public IActionResult MenuProductCategory()
		{
			var items = _context.ProductCategory.ToList();
			return PartialView("_MenuProductCategory", items);
		}
		public IActionResult MenuLeft(int? id)
		{
			if (id != null)
			{
				ViewBag.CateId = id;
			}
			var items = _context.ProductCategory.ToList();
			return PartialView("_MenuLeft", items);
		}

		public IActionResult MenuArrivals()
		{
			var items = _context.ProductCategory.ToList();
			return PartialView("_MenuArrivals", items);
		}
        [HttpPost]
        public ActionResult Subscribe(Subscribe req)
        {
            if (ModelState.IsValid)
            {
                _context.Subscribe.Add(new Subscribe { Email = req.Email, CreatedDate = DateTime.Now });
                _context.SaveChanges();
                return Json(new { Success = true });
            }
            return Json(new { success = false });
        }
    }
}
