using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public  IActionResult Index(int? page = 1)
        {
            
            IEnumerable<Product> items =  _context.Product.Include(x=>x.ProductCategory).Include(x=>x.ProductImages).OrderByDescending(x => x.ProductId);
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
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
                       
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products");
                        FileInfo fileInfo = new FileInfo(fileImage.FileName);

                        if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".jpeg")
                        {
                            countImage++;
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string filename = fileImage.FileName;
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
        public async Task<IActionResult> Edit(int id, Product product, ICollection<IFormFile> files)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            ModelState.ClearValidationState("ProductCategory");
            ModelState.MarkFieldValid("ProductCategory");
            ModelState.ClearValidationState("files");
            ModelState.MarkFieldValid("files");
            if (ModelState.IsValid)
            {
                try
                {
                    int countImage = 0;
                    foreach (var fileImage in files)
                    {
                        if (fileImage.FileName != null)
                        {

                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products");
                            FileInfo fileInfo = new FileInfo(fileImage.FileName);

                            if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".jpeg")
                            {
                                countImage++;
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                string filename = fileImage.FileName;
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

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
