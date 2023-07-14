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
using WebBanHangOnline.Models.EF;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ProductCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\ProductCategory");
        public ProductCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductCategories
        public IActionResult Index(string Searchtext, int? page = 1)
        {
            IEnumerable<ProductCategory> items = _context.ProductCategory.OrderByDescending(x => x.ProductCategoryId);
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

        // GET: Admin/ProductCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductCategory == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategory
                .FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
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
              
                _context.ProductCategory.Add(productCategory);
                await _context.SaveChangesAsync();
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
            if (id == null || _context.ProductCategory == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategory.FindAsync(id);
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
            var productCategory_Edit = _context.ProductCategory.Where(x => x.ProductCategoryId == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
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
                    _context.Update(productCategory_Edit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.ProductCategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _context.ProductCategory.Find(id);
            if (item != null )
            {
                // Check product in Product Category
                var productItem = _context.Product.Where(x=>x.ProductCategoryId ==id).ToList();
                if (productItem.Count ==0)
                {
                    FileInfo file = new FileInfo(path + "\\" + item.Icon);
                    if (file.Exists)//check file exsit or not  
                    {
                        file.Delete();
                    }
                    _context.ProductCategory.Remove(item);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }            
            }

            return Json(new { success = false });
        }

      
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _context.ProductCategory.Find(Convert.ToInt32(item));
                        if (obj != null)
                        {
                            // Check product in Product Category
                            var productItem = _context.Product.Where(x=>x.ProductCategoryId == obj.ProductCategoryId).ToList();
                            if (productItem.Count == 0)
                            {
                                FileInfo file = new FileInfo(path + "\\" + obj.Icon);
                                if (file.Exists)//check file exsit or not  
                                {
                                    file.Delete();
                                }
                                _context.ProductCategory.Remove(obj);
                                _context.SaveChanges();                           
                            }                           
                        }                                             
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        private bool ProductCategoryExists(int id)
        {
          return (_context.ProductCategory?.Any(e => e.ProductCategoryId == id)).GetValueOrDefault();
        }

       
    }
}
