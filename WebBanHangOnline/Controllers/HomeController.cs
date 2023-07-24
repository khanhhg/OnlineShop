using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.IRepository;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;		
        IProductsRepository _IProducts;
        IProductCategoriesRepository _IproductCategories;
        public HomeController(ILogger<HomeController> logger, IProductsRepository productsRepository,IProductCategoriesRepository productCategories)
        {
            _logger = logger;
            _IProducts = productsRepository; 
            _IproductCategories = productCategories;
         }
        public async Task<IActionResult> Index()
        {
            HomeViewModel objHomeView = new HomeViewModel();
            var itemSales = await _IProducts.GetProduts_By_Caterory(0, 12) ;
			var item_by_Category = await _IProducts.GetProduts_Promotion(0,12);
            var CateroryArrivals = await _IproductCategories.GetAll();

            objHomeView.CateroryArrivals = CateroryArrivals;
            objHomeView.ProductSales = itemSales;
			objHomeView.Product_ByCategory = item_by_Category;
			return View(objHomeView);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult PartialCounter()
        {
            var item = new CounterModel();

            //ViewBag.Visitors_online = HttpContext.Request.PathBase["visitors_online"];
            //var hn = HttpContext.Application["Today"];
            //item.Today = HttpContext.Application["Today"].ToString();
            //item.Yesterday = HttpContext.Application["Yesterday"].ToString();
            //item.ThisWeek = HttpContext.Application["ThisWeek"].ToString();
            //item.LastWeek = HttpContext.Application["LastWeek"].ToString();
            //item.ThisMonth = HttpContext.Application["ThisMonth"].ToString();
            //item.LastMonth = HttpContext.Application["LastMonth"].ToString();
            //item.All = HttpContext.Application["All"].ToString();
            return PartialView(item);
        }

    }
}