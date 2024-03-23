using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using AspNetCoreHero.ToastNotification.Abstractions;
using WebBanHangOnline.Services.IRepository;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Data.Models.Dtos;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        IProductsRepository _product;
        private readonly INotyfService _toastNotification;

        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products");
        public ProductsController(IProductsRepository productsRepository, INotyfService toastNotification)
        {
            _product = productsRepository;     
            _toastNotification = toastNotification;
        }

        public async  Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            
            IEnumerable<Product> items = await _product.GetAll();
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Title.Contains(Searchtext) || x.ProductCategory.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.ProductCategory = new SelectList(await _product.GetAllProductCategory(), "ProductCategoryId", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto product, ICollection<IFormFile> files)
        {         
            if (ModelState.IsValid)
            {

                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                product.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                product.Detail = string.IsNullOrWhiteSpace(product.Detail)?"": product.Detail.Replace("../..", String.Empty);
             var productNew =  await _product.Add(product);

                int countImage = 0;
                foreach (var fileImage in files)
                {
                    if (fileImage != null)
                    {
                        string fileName = Common.Common.SaveFile(path, fileImage);
                        var _image = new ProductImage
                        {
                            ProductId = productNew.ProductId,
                            Image = fileName,
                            IsDefault = true
                        };

                        if (countImage == 1)
                        {
                            await _product.AddImage(_image);                         
                        }
                        else
                        {
                            await _product.AddImage(_image);
                            
                        }
                    }
                }              
                _toastNotification.Success("Create Product Success");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.Error("Create Product Failed");
                ViewBag.ProductCategory = new SelectList(await _product.GetAllProductCategory(), "ProductCategoryId", "Title");
                return View(product);
            }
         
        }
     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _product.GetDto((int)id) == null)
            {
                return NotFound();
            }
            else
            {
                var product = await _product.GetDto((int)id);
                ViewBag.ProductCategory = new SelectList(await _product.GetAllProductCategory(), "ProductCategoryId", "Title");
                return View(product);
            }                  
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDto product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {                              
                 product.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                 product.ModifiedDate = DateTime.Now;
                product.Detail = string.IsNullOrWhiteSpace(product.Detail) ? "" : product.Detail.Replace("../..", String.Empty);
                await _product.Update(product);
                _toastNotification.Success("Update Product Success");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.Error("Update Product Failed");
                ViewBag.ProductCategory = new SelectList(await _product.GetAllProductCategory(), "ProductCategoryId", "Title");
                return View(product);
            }         
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _product.Get(id);
            if (item != null)
            {
                var checkImg = item.ProductImages.Where(x => x.ProductId == item.ProductId);
                if (checkImg != null)
                {
                    foreach (var img in checkImg)
                    {
                        FileInfo file = new FileInfo(path + "\\" + img.Image);
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                       await _product.DeleteImage(img);
                    }
                }
                await _product.Delete(item);
               
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }        
        }
        [HttpPost]
        public async Task<ActionResult> DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var objProduct = await _product.Get(Convert.ToInt32(item));
                        if (objProduct != null)
                        {
                            var checkImg = objProduct.ProductImages.Where(x => x.ProductId == objProduct.ProductId);
                            if (checkImg != null)
                            {
                                foreach (var img in checkImg)
                                {
                                    FileInfo file = new FileInfo(path + "\\" + img.Image);
                                    if (file.Exists)//check file exsit or not  
                                    {
                                        file.Delete();
                                    }
                                    await _product.DeleteImage(img);
                                }
                            }
                           await _product.Delete(objProduct);
                                                    
                        }
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<ActionResult> IsActive(int id)
        {
            var item = await _product.Get(id);
            if (item != null)
            {
                await _product.IsActive(item);
                return Json(new { success = true, isAcive = item.IsActive });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<ActionResult> IsHome(int id)
        {
            var item = await _product.Get(id);
            if (item != null)
            {
                await _product.IsHome(item);
                return Json(new { success = true, isHome = item.IsHome });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<ActionResult> IsSale(int id)
        {
            var item = await _product.Get(id);
            if (item != null)
            {
                await _product.IsSale(item);
                return Json(new { success = true, isSale = item.IsSale });
            }
            else
            {
                return Json(new { success = false });
            }        
        }     
        public async Task<IActionResult> listProductImage(int id)
        {
            ViewBag.ProductId = id;
            var items = await _product.GetImageByProduct(id);
            return View(items);
        }

        public IActionResult createImage(int? id)
        {
            ViewBag.ProductId = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> createImage(int id, ICollection<IFormFile> fileImages)
        {                   
            if (ModelState.IsValid)
            {
                foreach (var fileImage in fileImages)
                {
                    if (fileImage != null)
                    {
                        string fileName = Common.Common.SaveFile(path, fileImage);
                        ProductImage objImg = new ProductImage();
                        objImg.ProductId = id;
                        objImg.Image = fileName;
                        objImg.IsDefault = false;
                        await _product.AddImage(objImg);
                        _toastNotification.Success("Add Image Product Success");
                    }
                }
            }
           return LocalRedirect("/admin/products/listProductImage/" + id);
        }
        public IActionResult editImage(int? id)
        {
            var items = _product.GetImage((int)id);
            return View(items);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editImage(int id, IFormFile fileImage)
        {
            //if (id != product.ProductId)
            //{
            //    return NotFound();
            //}
            int _producID = 0;
            if (ModelState.IsValid)
            {
                if (fileImage != null)
                {
                    string fileName = Common.Common.SaveFile(path, fileImage);
                    var objImage = await _product.GetImage((int)id);
                    objImage.Image = fileName;
                    await _product.UpdateImage(objImage);
                    _producID = objImage.ProductId;
                    _toastNotification.Success("Update Image Product Success");
                }
            }
            return LocalRedirect("/admin/products/listProductImage/" + _producID);
        }
        [HttpPost]
        public async Task<ActionResult> deleteImage(int id)
        {                 
            var item = await _product.GetImage(id);
            if (item != null)
            {
                FileInfo file = new FileInfo(path +"\\"+ item.Image);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                await _product.DeleteImage(item);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }        
        }
        [HttpPost]
        public async Task<ActionResult> deleteAllImage(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var objImage = await _product.GetImage(Convert.ToInt32(item));
                        if (objImage != null)
                        {
                            FileInfo file = new FileInfo(path + "\\" + objImage.Image);
                            if (file.Exists)//check file exsit or not  
                            {
                                file.Delete();
                            }
                            await _product.DeleteImage(objImage);
                        }
                    }
                }
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }       
        }
        [HttpPost]
        public async Task<ActionResult> IsDefault(int id)
        {
            var item = await _product.GetImage(id);
            if (item != null)
            {
               await _product.IsDefault(item);
                return Json(new { success = true, IsDefault = item.IsDefault });
            }
            else
            {
                return Json(new { success = false });
            }        
        }
        [HttpPost]
        public ActionResult TinyMceUpload(IFormFile file)
        {
            string targetFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products\\details");
            if (file != null)
            {
                var location = "/images/products/details/" + Common.Common.SaveFile(targetFolder, file);
                return Json(new { location });
            }
            else
            {
                var Error = "File not found !";
                return Json(new { Error });
            }
        }
    }
}
