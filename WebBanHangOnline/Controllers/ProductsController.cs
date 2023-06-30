using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;
        public ProductsController(ApplicationDbContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public ActionResult Index()
        {          
            var items = _context.Product.Include(x => x.ProductImages).OrderByDescending(x => x.ProductId).ToList();

            return View(items);
        }

        public ActionResult Detail(int id)
        {
            var item = _context.Product.Where(x=>x.ProductId ==id).Include(x=>x.ProductCategory).Include(x => x.ProductImages).FirstOrDefault();
            if (item != null)
            {
                _context.Product.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                _context.Entry(item).Property(x => x.ViewCount).IsModified = true;
                _context.SaveChanges();
            }

            return View(item);
        }
        public ActionResult ProductCategory(int id)
        {
            var items = _context.Product.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = _context.ProductCategory.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = _context.Product.Include(x => x.ProductImages).Include(x => x.ProductCategory).Take(12).ToList();
            return PartialView("_Partial_ItemsByCateId", items);
        }

        public async Task<IActionResult> Partial_ProductSales()
        {
            //var items = _context.Product.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
			var items = await _context.Product.Include(x=>x.ProductImages).Include(x=>x.ProductCategory).Take(12).ToListAsync();
            return PartialView("_Partial_ProductSales", items);
        }
    }
}
