using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;
using WebBanHangOnline.Models;
using WebBanHangOnline.Common;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebBanHangOnline.Controllers
{

    public class ShoppingCartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public ShoppingCartController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;    
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
        [Authorize]
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

        [Authorize]
        public ActionResult Partial_Item_CheckOut()
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                return PartialView("_Partial_Item_CheckOut", cart.Items);
            }
            return PartialView("_Partial_Item_CheckOut");
        }

		[HttpGet]
		public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
				HttpContext.Session.SetObjectAsJson("Cart", cart);
				return PartialView("_Partial_Item_Cart", cart.Items);
            }
            return PartialView("_Partial_Item_Cart");
        }

		[HttpGet]
		public ActionResult ShowCount()
        {
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
				HttpContext.Session.SetObjectAsJson("Cart", cart);
				return Json(new { count = cart.Items.Count });
            }
			return Json(new { count = 0 });
            //return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
		}

        [Authorize]
        public ActionResult Partial_CheckOut()
        {
            string vUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var objUser = _context.UserProfile.Where(x => x.UserID == vUserID).FirstOrDefault();
            if (objUser !=null)
            {
                OrderViewModel item = new OrderViewModel();
                item.CustomerName = objUser.CustomerName;
                item.Address = objUser.Address;
                item.Phone = objUser.PhoneNumber;
                item.Email = objUser.Email;
                return PartialView("_Partial_CheckOut",item);
            }
            else
            {
                return PartialView("_Partial_CheckOut");
            }          
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
			string path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Content\\templates\\send1.html");
			string path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Content\\templates\\send2.html");
			var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");              
				if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    order.Email = req.Email;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.ModifiedDate = DateTime.Now;
                    order.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    order.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    //order.E = req.CustomerName;
                    _context.Order.Add(order);
                    _context.SaveChanges();

                    // Update UnitsOnOrder for Product
                    foreach (var it in order.OrderDetails)
                    {
                        var objProduct = _context.Product.Where(x => x.ProductId == it.ProductId).FirstOrDefault();
                        if (objProduct != null)
                        {
                            _context.Product.Attach(objProduct);
                            objProduct.UnitsOnOrder = objProduct.UnitsOnOrder + it.Quantity;
                            _context.Entry(objProduct).Property(x => x.UnitsOnOrder).IsModified = true;                        
                            _context.SaveChanges(true);
                        }
                    }

                    //send mail to customer
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + WebBanHangOnline.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    TongTien = thanhtien;
                    string contentCustomer = System.IO.File.ReadAllText(path2);
                    contentCustomer = contentCustomer.Replace("{{OrderCode}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{Product}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{OrderDate}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{CustomerName}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{Address}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{Amount}}", WebBanHangOnline.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TotalPrice}}", WebBanHangOnline.Common.Common.FormatNumber(TongTien, 0));
                    WebBanHangOnline.Common.Common.SendMail("ShopOnline", "Order #" + order.Code, contentCustomer.ToString(), req.Email);

                    string contentAdmin = System.IO.File.ReadAllText(path1);
                    contentAdmin = contentAdmin.Replace("{{OrderCode}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{Product}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{OrderDate}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{CustomerName}}", order.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    contentAdmin = contentAdmin.Replace("{{Address}}", order.Address);
                    contentAdmin = contentAdmin.Replace("{{Amount}}", WebBanHangOnline.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TotalPrice}}", WebBanHangOnline.Common.Common.FormatNumber(TongTien, 0));
                    WebBanHangOnline.Common.Common.SendMail("ShopOnline", "New Order #" + order.Code, contentAdmin.ToString(), "nhkhanhkc@gmail.com");
                    cart.ClearCart();
					HttpContext.Session.SetObjectAsJson("Cart", cart);

					return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }

        [HttpPost]
		public ActionResult AddToCart(int id,int quantity)
        {
            bool QuantityCheck = true;
            var code = new { Success = false, msg = "",heading="", code = -1, count = 0 };
           
            var checkProduct = _context.Product.Include(x=>x.ProductCategory).Where(x=>x.UnitsInStock > x.UnitsOnOrder).Include(x=>x.ProductImages).FirstOrDefault(x => x.ProductId == id);
            if (checkProduct != null)
            {
                int Units = UnitsInStock(id);
                ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                else
                {
                  var CartItems =  cart.Items.FirstOrDefault(x => x.ProductId == id);
                    if (CartItems !=null)
                    {
                        if (Units <= CartItems.Quantity)
                        {
                            QuantityCheck = false;
                        }
                    }
                  
                }
                // Check Product In Stock
                if (QuantityCheck ==true)
                {
                    ShoppingCartItem item = new ShoppingCartItem
                    {
                        ProductId = checkProduct.ProductId,
                        ProductName = checkProduct.Title,
                        CategoryName = checkProduct.ProductCategory.Title,
                        Alias = checkProduct.Alias,
                        Quantity = quantity
                    };
                    if (checkProduct.ProductImages.Where(x => x.IsDefault == true).FirstOrDefault() != null)
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
                    code = new { Success = true, msg = "Product added to cart successfully!",heading="success", code = 1, count = cart.Items.Count };
                }
                else
                {
                    code = new { Success = true, msg = "Product is out of stock!", heading = "warning", code = 1, count = cart.Items.Count };
                }
               
            }
          
			return Json(code);
		}

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            var code = new { Success = false, msg = "", heading = "", code = -1, count = 0 };
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
			if (cart != null)
            {
                int Units = UnitsInStock(id);
                if (Units < quantity)
                {                  
                    quantity = Units;
                    code = new { Success = true, msg = "Product is out of stock!", heading = "warning", code = 1, count = quantity };
                }
                else
                {
                    code = new { Success = true, msg = "Product update to cart successfully!", heading = "success", code = 1, count = quantity };
                }
              
                cart.UpdateQuantity(id, quantity);
				HttpContext.Session.SetObjectAsJson("Cart", cart);
				return Json(code);
            }
            else
            {
                return Json(new { Success = false });
            }         
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, count = 0 };

            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
					HttpContext.Session.SetObjectAsJson("Cart", cart);
					code = new { Success = true, msg = "Product update to cart successfully!", code = 1, count = cart.Items.Count };
                }
            }
            else
            {
                code = new { Success = true, msg = "Product is out of stock!", code = 1, count = cart.Items.Count };
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
				HttpContext.Session.SetObjectAsJson("Cart", cart);
				return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        public int UnitsInStock(int Id)
        {
            int Units = 0;
            var objProduct = _context.Product.Where(x => x.ProductId == Id).FirstOrDefault();
            if (objProduct !=null)
            {
                 Units = objProduct.UnitsInStock - objProduct.UnitsOnOrder;
            }
            return Units;
        }
    }
}
