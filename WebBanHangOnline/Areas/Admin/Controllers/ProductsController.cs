using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;
using X.PagedList;
using WebBanHangOnline.Common;


namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products");
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public  IActionResult Index(string Searchtext, int? page = 1)
        {
            
            IEnumerable<Product> items =  _context.Product.Include(x=>x.ProductCategory).Include(x=>x.ProductImages).OrderByDescending(x => x.ProductId);
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

		// GET: Admin/Products/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewBag.ProductCategory = new SelectList(_context.ProductCategory.ToList(), "ProductCategoryId", "Title");
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
                    if (fileImage.FileName != null)
                    {
                       
                        //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products");
                        FileInfo fileInfo = new FileInfo(fileImage.FileName);

                        if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".jpeg")
                        {
                            countImage++;
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
                            if (countImage == 1)
                            {
                                product.ProductImages.Add(new ProductImage
                                {
                                    ProductId = product.ProductId,
                                    Image = filename,
                                    IsDefault = true
                                });
                                product.Image = filename;
                            }
                            else
                            {
                                product.ProductImages.Add(new ProductImage
                                {
                                    ProductId = product.ProductId,
                                    Image = filename,
                                    IsDefault = false
                                });
                            }                          
                        }
                    }
                }
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                product.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                product.ProductCategory = _context.ProductCategory.Where(x => x.ProductCategoryId == product.ProductCategoryId).FirstOrDefault();

                _context.Product.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ProductCategory = new SelectList(_context.ProductCategory.ToList(), "ProductCategoryId", "Title");
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.ProductCategory = new SelectList(_context.ProductCategory.ToList(), "ProductCategoryId", "Title");
            return View(product);
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
                try
                {
                    //int countImage = 0;
                    //foreach (var fileImage in files)
                    //{
                    //    if (fileImage.FileName != null)
                    //    {
                          
                    //        FileInfo fileInfo = new FileInfo(fileImage.FileName);

                    //        if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".jpeg")
                    //        {
                    //            countImage++;
                    //            if (!Directory.Exists(path))
                    //            {
                    //                Directory.CreateDirectory(path);
                    //            }
                    //            string filename = Common.Common.RandomString(12) + fileInfo.Extension;
                    //            string fileNameWithPath = Path.Combine(path, filename);
                    //            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    //            {
                    //                fileImage.CopyTo(stream);
                    //            }
                    //            if (countImage == 1)
                    //            {
                    //                product.ProductImages.Add(new ProductImage
                    //                {
                    //                    ProductId = product.ProductId,
                    //                    Image = filename,
                    //                    IsDefault = true
                    //                });
                    //                product.Image = filename;
                    //            }
                    //            else
                    //            {
                    //                product.ProductImages.Add(new ProductImage
                    //                {
                    //                    ProductId = product.ProductId,
                    //                    Image = filename,
                    //                    IsDefault = false
                    //                });
                    //            }
                    //        }
                    //    }
                    //}
                    product.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    product.ModifiedDate = DateTime.Now;
                    _context.Update(product);
                    await _context.SaveChangesAsync();            
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewBag.ProductCategory = new SelectList(_context.ProductCategory.ToList(), "ProductCategoryId", "Title");
            return View(product);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _context.Product.Find(id);
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
                        _context.ProductImage.Remove(img);
                        _context.SaveChanges();
                    }
                }
                _context.Product.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
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
                        var objProduct = _context.Product.Find(item);
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
                                    _context.ProductImage.Remove(img);
                                    _context.SaveChanges();
                                }
                            }
                            _context.Product.Remove(objProduct);
                            _context.SaveChanges();                         
                        }
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _context.Product.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { success = true, isAcive = item.IsActive });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = _context.Product.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                _context.Entry(item).State = EntityState.Modified;
                 _context.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = _context.Product.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { success = true, IsSale = item.IsSale });
            }

            return Json(new { success = false });
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
        public IActionResult listProductImage(int id)
        {
            ViewBag.ProductId = id;
            var items = _context.ProductImage.Where(x => x.ProductId == id).ToList();
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
            //if (id != product.ProductId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                foreach (var fileImage in fileImages)
                {
                    if (fileImage.FileName != null)
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

                            ProductImage objImg = new ProductImage();

                            objImg.ProductId = id;
                            objImg.Image = filename;
                            objImg.IsDefault = false;
                            _context.ProductImage.Add(objImg);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
           return LocalRedirect("/admin/products/listProductImage/" + id);
        }
        public IActionResult editImage(int? id)
        {
          
            var items = _context.ProductImage.Where(x => x.ProductImageId == id).FirstOrDefault();
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
                if (fileImage.FileName != null)
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
                        var objImage = _context.ProductImage.Where(x => x.ProductImageId == id).FirstOrDefault();
                        objImage.Image = filename;
                        _context.Update(objImage);                 
                        await _context.SaveChangesAsync();
                        _producID = objImage.ProductId;
                    }
                }
            }
            return LocalRedirect("/admin/products/listProductImage/" + _producID);
        }
        [HttpPost]
        public ActionResult deleteImage(int id)
        {
                   
            var item = _context.ProductImage.Find(id);
            if (item != null)
            {
                FileInfo file = new FileInfo(path +"\\"+ item.Image);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                _context.ProductImage.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }        
        }
        [HttpPost]
        public ActionResult deleteAllImage(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var objImage = _context.ProductImage.Find(Convert.ToInt32(item));
                        if (objImage != null)
                        {
                            FileInfo file = new FileInfo(path + "\\" + objImage.Image);
                            if (file.Exists)//check file exsit or not  
                            {
                                file.Delete();
                            }
                            _context.ProductImage.Remove(objImage);
                            _context.SaveChanges();
                        }
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsDefault(int id)
        {
            var item = _context.ProductImage.Find(id);
            if (item != null)
            {
                item.IsDefault = !item.IsDefault;
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { success = true, IsDefault = item.IsDefault });
            }

            return Json(new { success = false });
        }
    }
}
