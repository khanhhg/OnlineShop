﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;
		private readonly INotyfService _toastNotification;
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, INotyfService toastNotification)
        {
            _logger = logger;
			_context = context;
			_toastNotification = toastNotification;
		}

        public async Task<IActionResult> Index()
        {
            HomeViewModel objHomeView = new HomeViewModel();
			var itemSales = await _context.Product.Include(x => x.ProductImages).Include(x => x.ProductCategory).Take(12).ToListAsync();
			var item_by_Category = await _context.Product.Include(x => x.ProductImages).Include(x => x.ProductCategory).Take(12).ToListAsync();
            var CateroryArrivals = await _context.ProductCategory.ToListAsync();

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

            //ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
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