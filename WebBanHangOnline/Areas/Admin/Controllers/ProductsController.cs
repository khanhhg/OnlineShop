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

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        IProductsRepository _IProducts;
        IProductCategoriesRepository _IProductCategories;

        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products");
        public ProductsController(IProductsRepository productsRepository, IProductCategoriesRepository productCategoriesRepository)
        {
            _IProducts = productsRepository;
            _IProductCategories = productCategoriesRepository;
        }

        // GET: Admin/Products
        public async  Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            
            IEnumerable<Product> items = await _IProducts.GetAll();
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

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ProductCategory = new SelectList(await _IProductCategories.GetAll(), "ProductCategoryId", "Title");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                            await _IProducts.AddImage(new ProductImage
                            {
                                ProductId = product.ProductId,
                                Image = fileName,
                                IsDefault = true
                            });
                            product.Image = fileName;
                        }
                        else
                        {
                            await _IProducts.AddImage(new ProductImage
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
                await _IProducts.Add(product);              
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ProductCategory = new SelectList(await _IProductCategories.GetAll(), "ProductCategoryId", "Title");
                return View(product);
            }
         
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _IProducts.Get((int)id) == null)
            {
                return NotFound();
            }
            else
            {
                var product = await _IProducts.Get((int)id);
                ViewBag.ProductCategory = new SelectList(await _IProductCategories.GetAll(), "ProductCategoryId", "Title");
                return View(product);
            }                  
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                await _IProducts.Update(product);                                                   
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ProductCategory = new SelectList(await _IProductCategories.GetAll(), "ProductCategoryId", "Title");
                return View(product);
            }         
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _IProducts.Get(id);
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
                       await _IProducts.DeleteImage(img);
                    }
                }
                await _IProducts.Delete(item);
               
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
                        var objProduct = await _IProducts.Get(Convert.ToInt32(item));
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
                                    await _IProducts.DeleteImage(img);
                                }
                            }
                           await _IProducts.Delete(objProduct);
                                                    
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
            var item = await _IProducts.Get(id);
            if (item != null)
            {
                await _IProducts.IsActive(item);
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
            var item = await _IProducts.Get(id);
            if (item != null)
            {
                await _IProducts.IsHome(item);
                return Json(new { success = true, isAcive = item.IsActive });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<ActionResult> IsSale(int id)
        {
            var item = await _IProducts.Get(id);
            if (item != null)
            {
                await _IProducts.IsSale(item);
                return Json(new { success = true, isAcive = item.IsActive });
            }
            else
            {
                return Json(new { success = false });
            }        
        }     
        public async Task<IActionResult> listProductImage(int id)
        {
            ViewBag.ProductId = id;
            var items = await _IProducts.GetImageByProduct(id);
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
                        await _IProducts.AddImage(objImg);
                    }
                }
            }
           return LocalRedirect("/admin/products/listProductImage/" + id);
        }
        public IActionResult editImage(int? id)
        {
            var items = _IProducts.GetImage((int)id);
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
                    var objImage = await _IProducts.GetImage((int)id);
                    objImage.Image = fileName;
                    await _IProducts.UpdateImage(objImage);
                    _producID = objImage.ProductId;
                }
            }
            return LocalRedirect("/admin/products/listProductImage/" + _producID);
        }
        [HttpPost]
        public async Task<ActionResult> deleteImage(int id)
        {                 
            var item = await _IProducts.GetImage(id);
            if (item != null)
            {
                FileInfo file = new FileInfo(path +"\\"+ item.Image);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                await _IProducts.DeleteImage(item);
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
                        var objImage = await _IProducts.GetImage(Convert.ToInt32(item));
                        if (objImage != null)
                        {
                            FileInfo file = new FileInfo(path + "\\" + objImage.Image);
                            if (file.Exists)//check file exsit or not  
                            {
                                file.Delete();
                            }
                            await _IProducts.DeleteImage(objImage);
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
            var item = await _IProducts.GetImage(id);
            if (item != null)
            {
               await _IProducts.IsDefault(item);
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
