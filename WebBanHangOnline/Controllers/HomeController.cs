using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
            // ID = 0 => Get all product : Id =! 0 =>  where ProductCatgoryID = Id
            // Top = 0 => Get all product : Top > 0 =>  Take Top
            // IsHome = false => Get All : IsHome = True => where Ishome= True
            // IsHome = false => Get All : IsSale = True => where IsSale= True

            HomeViewModel objHomeView = new HomeViewModel();
            var item_by_Category  = await _IProducts.GetViewProducts(0,10,true,false);
			var itemSales = await _IProducts.GetViewProducts(0, 10, true, false);
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