using System;
using System.Data;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data.IRepository;
using WebBanHangOnline.Models.EF;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        //private readonly ApplicationDbContext _context;
        ICategoriesRepository _ICategories;
        private readonly INotyfService _toastNotification;
        public CategoriesController(ICategoriesRepository iCategoriesRepository,INotyfService toastNotification)
        {
            _ICategories = iCategoriesRepository;
            _toastNotification = toastNotification;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            IEnumerable<Category> items = await _ICategories.GetAll();
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Title.Contains(Searchtext)).OrderByDescending(x=>x.Position);
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedDate = DateTime.Now;
                category.ModifiedDate = DateTime.Now;
                category.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier); 
                category.IsActive = true;
                await _ICategories.Add(category);
                _toastNotification.Success("Create Category Success");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.Error("Create Category Failed");
                return View(category);
            }      
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _ICategories.Get((int)id) == null)
            {
                return NotFound();
            }

            var category =  await _ICategories.Get((int)id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                category.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                category.ModifiedDate = DateTime.Now;
               await _ICategories.Update(category);
                _toastNotification.Success("Update Category Success");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.Error("Update Category Failed");
                return View(category);
            }         
        }
      
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _ICategories.Get(id);
            if (item != null)
            {
                //var DeleteItem = db.Categories.Attach(item);
                await _ICategories.Delete(item); 
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }          
        }   
    }
}
