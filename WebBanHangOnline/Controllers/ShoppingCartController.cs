using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;
using WebBanHangOnline.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using WebBanHangOnline.Common;

namespace WebBanHangOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;
        public ShoppingCartController(ILogger<HomeController> logger, ApplicationDbContext context, INotyfService toastNotification)
        {
            _logger = logger;
            _context = context;
            _toastNotification = toastNotification;
        }

        public ActionResult Index()
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult CheckOut()
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult Partial_Item_CheckOut()
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }


        public ActionResult ShowCount()
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            //if (cart != null)
            //{
            //    return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            //}
            //return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
            return null;
        }

        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CheckOut(OrderViewModel req)
        //{
        //    var code = new { Success = false, Code = -1 };
        //    if (ModelState.IsValid)
        //    {
        //        ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
        //        if (cart != null)
        //        {
        //            Order order = new Order();
        //            order.CustomerName = req.CustomerName;
        //            order.Phone = req.Phone;
        //            order.Address = req.Address;
        //            order.Email = req.Email;
        //            cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
        //            {
        //                ProductId = x.ProductId,
        //                Quantity = x.Quantity,
        //                Price = x.Price
        //            }));
        //            order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
        //            order.TypePayment = req.TypePayment;
        //            order.CreatedDate = DateTime.Now;
        //            order.ModifiedDate = DateTime.Now;
        //            order.CreatedBy = req.Phone;
        //            Random rd = new Random();
        //            order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
        //            //order.E = req.CustomerName;
        //            _context.Order.Add(order);
        //            _context.SaveChanges();
        //            //send mail cho khachs hang
        //            var strSanPham = "";
        //            var thanhtien = decimal.Zero;
        //            var TongTien = decimal.Zero;
        //            foreach (var sp in cart.Items)
        //            {
        //                strSanPham += "<tr>";
        //                strSanPham += "<td>" + sp.ProductName + "</td>";
        //                strSanPham += "<td>" + sp.Quantity + "</td>";
        //                strSanPham += "<td>" + WebBanHangOnline.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
        //                strSanPham += "</tr>";
        //                thanhtien += sp.Price * sp.Quantity;
        //            }
        //            TongTien = thanhtien;
        //            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
        //            contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
        //            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
        //            contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
        //            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
        //            contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
        //            contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
        //            contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
        //            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", WebBanHangOnline.Common.Common.FormatNumber(thanhtien, 0));
        //            contentCustomer = contentCustomer.Replace("{{TongTien}}", WebBanHangOnline.Common.Common.FormatNumber(TongTien, 0));
        //            WebBanHangOnline.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);

        //            string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
        //            contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
        //            contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
        //            contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
        //            contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
        //            contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
        //            contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
        //            contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
        //            contentAdmin = contentAdmin.Replace("{{ThanhTien}}", WebBanHangOnline.Common.Common.FormatNumber(thanhtien, 0));
        //            contentAdmin = contentAdmin.Replace("{{TongTien}}", WebBanHangOnline.Common.Common.FormatNumber(TongTien, 0));
        //            WebBanHangOnline.Common.Common.SendMail("ShopOnline", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
        //            cart.ClearCart();
        //            return RedirectToAction("CheckOutSuccess");
        //        }
        //    }
        //    return Json(code);
        //}

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
           
            var checkProduct = _context.Product.FirstOrDefault(x => x.ProductId == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.ProductId,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImages.Where(x=>x.IsDefault ==true).FirstOrDefault() != null)
                {
                    item.ProductImg = checkProduct.ProductImages.Where(x => x.IsDefault == true).FirstOrDefault().Image;
                }
                item.Price = (decimal)checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                //Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm vào giở hàng thành công!", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }



        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
}
