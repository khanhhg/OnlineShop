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
            var item_by_Category  = await _IProducts.GetProduts_By_Caterory(0, 10) ;
			var itemSales = await _IProducts.GetProduts_Promotion(0,10);
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
    }
}