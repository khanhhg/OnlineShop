using System;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Models.EF;
using X.PagedList;
using WebBanHangOnline.Data.IRepository;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        IProductsRepository _productsRepository;
        private readonly INotyfService _toastNotification;

        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products");
        public ProductsController(IProductsRepository productsRepository, INotyfService toastNotification)
        {
            _productsRepository = productsRepository;     
            _toastNotification = toastNotification;
        }

        public async  Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            
            IEnumerable<Product> items = await _productsRepository.GetAll();
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
            ViewBag.ProductCategory = new SelectList(await _productsRepository.GetAllProductCategory(), "ProductCategoryId", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, ICollection<IFormFile> files)
        {
            ModelState.ClearValidationState("ProductCategory");
            ModelState.MarkFieldValid("ProductCategory");
            if (ModelState.IsValid)
            {
                int countImage = 0;
                foreach (var fileImage in files)
                {
                    if (fileImage != null)
                    {
                        string fileName = Common.Common.SaveFile(path, fileImage);
                        if (countImage == 1)
                        {
                            await _productsRepository.AddImage(new ProductImage
                            {
                                ProductId = product.ProductId,
                                Image = fileName,
                                IsDefault = true
                            });
                            product.Image = fileName;
                        }
                        else
                        {
                            await _productsRepository.AddImage(new ProductImage
                            {
                                ProductId = product.ProductId,
                                Image = fileName,
                                IsDefault = false
                            });
                        }
                    }
                }
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                product.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);  
                product.Detail = product.Detail.Replace("../..", String.Empty);
                await _productsRepository.Add(product);
                _toastNotification.Success("Create Product Success");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.Error("Create Product Failed");
                ViewBag.ProductCategory = new SelectList(await _productsRepository.GetAllProductCategory(), "ProductCategoryId", "Title");
                return View(product);
            }
         
        }
     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _productsRepository.Get((int)id) == null)
            {
                return NotFound();
            }
            else
            {
                var product = await _productsRepository.Get((int)id);
                ViewBag.ProductCategory = new SelectList(await _productsRepository.GetAllProductCategory(), "ProductCategoryId", "Title");
                return View(product);
            }                  
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            ModelState.ClearValidationState("ProductCategory");
            ModelState.MarkFieldValid("ProductCategory");
            if (ModelState.IsValid)
            {                              
                 product.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                 product.ModifiedDate = DateTime.Now;
                 product.Detail = product.Detail.Replace("../..", String.Empty);
                await _productsRepository.Update(product);
                _toastNotification.Success("Update Product Success");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.Error("Update Product Failed");
                ViewBag.ProductCategory = new SelectList(await _productsRepository.GetAllProductCategory(), "ProductCategoryId", "Title");
                return View(product);
            }         
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _productsRepository.Get(id);
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
                       await _productsRepository.DeleteImage(img);
                    }
                }
                await _productsRepository.Delete(item);
               
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
                        var objProduct = await _productsRepository.Get(Convert.ToInt32(item));
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
                                    await _productsRepository.DeleteImage(img);
                                }
                            }
                           await _productsRepository.Delete(objProduct);
                                                    
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
            var item = await _productsRepository.Get(id);
            if (item != null)
            {
                await _productsRepository.IsActive(item);
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
            var item = await _productsRepository.Get(id);
            if (item != null)
            {
                await _productsRepository.IsHome(item);
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
            var item = await _productsRepository.Get(id);
            if (item != null)
            {
                await _productsRepository.IsSale(item);
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
            var items = await _productsRepository.GetImageByProduct(id);
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
                        await _productsRepository.AddImage(objImg);
                        _toastNotification.Success("Add Image Product Success");
                    }
                }
            }
           return LocalRedirect("/admin/products/listProductImage/" + id);
        }
        public IActionResult editImage(int? id)
        {
            var items = _productsRepository.GetImage((int)id);
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
                    var objImage = await _productsRepository.GetImage((int)id);
                    objImage.Image = fileName;
                    await _productsRepository.UpdateImage(objImage);
                    _producID = objImage.ProductId;
                    _toastNotification.Success("Update Image Product Success");
                }
            }
            return LocalRedirect("/admin/products/listProductImage/" + _producID);
        }
        [HttpPost]
        public async Task<ActionResult> deleteImage(int id)
        {                 
            var item = await _productsRepository.GetImage(id);
            if (item != null)
            {
                FileInfo file = new FileInfo(path +"\\"+ item.Image);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                await _productsRepository.DeleteImage(item);
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
                        var objImage = await _productsRepository.GetImage(Convert.ToInt32(item));
                        if (objImage != null)
                        {
                            FileInfo file = new FileInfo(path + "\\" + objImage.Image);
                            if (file.Exists)//check file exsit or not  
                            {
                                file.Delete();
                            }
                            await _productsRepository.DeleteImage(objImage);
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
            var item = await _productsRepository.GetImage(id);
            if (item != null)
            {
               await _productsRepository.IsDefault(item);
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
