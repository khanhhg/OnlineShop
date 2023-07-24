using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.IRepository;
using WebBanHangOnline.Models.EF;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ProductCategoriesController : Controller
    {
        IProductCategoriesRepository _IProductCategories;
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\ProductCategory");
        public ProductCategoriesController(IProductCategoriesRepository productCategoriesRepository)
        {
            _IProductCategories = productCategoriesRepository;
        }

        // GET: Admin/ProductCategories
        public async Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            IEnumerable<ProductCategory> items  = await _IProductCategories.GetAll() ;
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        // GET: Admin/ProductCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ProductCategory productCategory,IFormFile fileImage)
        {
            ModelState.ClearValidationState("Products");
            ModelState.MarkFieldValid("Products");
            if (ModelState.IsValid)
            {
                if (fileImage.FileName != null)
                {
                                   
                    FileInfo fileInfo = new FileInfo(fileImage.FileName);
                   
                    if(fileInfo.Extension ==".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".jpeg")
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filename = Common.Common.RandomString(12) + fileInfo.Extension;
                        string fileNameWithPath = Path.Combine(path, filename);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            fileImage.CopyTo(stream);
                        }
                        productCategory.Icon = filename;
                    }                   
                }
                productCategory.CreatedDate = DateTime.Now;
                productCategory.ModifiedDate = DateTime.Now;
                productCategory.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
              
                await _IProductCategories.Add(productCategory);         
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(productCategory);
            }
        }

        // GET: Admin/ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _IProductCategories.Get((int)id) == null)
            {
                return NotFound();
            }

            var productCategory =  await _IProductCategories.Get((int)id);
            if (productCategory == null)
            {
                return NotFound();
            }
            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCategory productCategory, IFormFile fileImage)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return NotFound();
            }
            ModelState.ClearValidationState("Products");
            ModelState.MarkFieldValid("Products");
            ModelState.ClearValidationState("fileImage");
            ModelState.MarkFieldValid("fileImage");
            var productCategory_Edit = await _IProductCategories.Get((int)id);
            if (ModelState.IsValid)
            {             
                    if (fileImage != null)
                    {                  
                        FileInfo fileInfo = new FileInfo(fileImage.FileName);

                        if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".jpeg")
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string filename = Common.Common.RandomString(12) + fileInfo.Extension;
                            string fileNameWithPath = Path.Combine(path, filename);
                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                fileImage.CopyTo(stream);
                            }
                            productCategory_Edit.Icon = filename;
                        }
                    }
                  
                    productCategory_Edit.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    productCategory_Edit.ModifiedDate = DateTime.Now;
                    productCategory_Edit.SeoTitle = productCategory.SeoTitle;
                    productCategory_Edit.SeoKeywords = productCategory.SeoKeywords;
                    productCategory_Edit.SeoDescription = productCategory.SeoDescription;
                    productCategory_Edit.Title = productCategory.Title;
                    productCategory_Edit.Description = productCategory.Description;
                   await _IProductCategories.Update(productCategory_Edit);                         
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(productCategory);
            }        
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _IProductCategories.Get(id);
            if (item != null )
            {
                FileInfo file = new FileInfo(path + "\\" + item.Icon);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                bool _delete = await _IProductCategories.Delete(item);
                if (_delete)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }                           
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
                        var obj = await _IProductCategories.Get(Convert.ToInt32(item));
                        if (obj != null)
                        {
                            FileInfo file = new FileInfo(path + "\\" + obj.Icon);
                            if (file.Exists)//check file exsit or not  
                            {
                                file.Delete();
                            }
                            await _IProductCategories.Delete(obj);
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
    }
}
