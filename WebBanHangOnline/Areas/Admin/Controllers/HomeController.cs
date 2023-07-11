using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Web.WebPages;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            string _fromDate = "";
            string _toDate = "";

            int day = (int)DateTime.Now.DayOfWeek;
         
            if (string.IsNullOrEmpty(fromDate))
            {
                _fromDate = DateTime.Now.AddDays(-day).ToString("dd/MM/yyyy");
            }
            if (string.IsNullOrEmpty(toDate))
            {
                _toDate = DateTime.Now.AddDays(7 - day).ToString("dd/MM/yyyy");
            }           
            DateTime startDate = DateTime.ParseExact(_fromDate, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(_toDate, "dd/MM/yyyy", null);


            var query = (from o in _context.Order
                        join od in _context.OrderDetail
                        on o.OrderId equals od.OrderId
                        join p in _context.Product
                        on od.ProductId equals p.ProductId
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        }).Where(x => x.CreatedDate >= startDate && x.CreatedDate < endDate).ToList();
           
            int dayDiff = (endDate - startDate).Days;                    
           List<ChartViewModel> result = new List<ChartViewModel>();
           for (int i= 1; i<= dayDiff; i++)
            {
                var Temp = query.Where(x => x.CreatedDate.Value.Date == startDate.AddDays(i).Date).ToList();
                ChartViewModel RowChart = new ChartViewModel();
                RowChart.Date = startDate.AddDays(i).Date;
                RowChart.TotalBuy = Temp.Sum(y => y.Quantity * y.OriginalPrice);
                RowChart.TotalSell = Temp.Sum(y => y.Quantity * y.Price) - RowChart.TotalBuy;
                result.Add(RowChart);
            }

            //var result = query.GroupBy(x => Microsoft.EntityFrameworkCore.EF.Functions.DateDiffDay(startDate, endDate)).Select(x => new
            //{
            //    Date = x.Key.Days(),
            //    TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
            //    TotalSell = x.Sum(y => y.Quantity * y.Price),
            //}).Select(x => new
            //{
            //    Date = x.Date,
            //    TotalBuy = x.TotalSell,
            //    TotalSell = x.TotalSell - x.TotalBuy
            //}).ToList();
            return Json(new { data = result });
        }
    }
}
