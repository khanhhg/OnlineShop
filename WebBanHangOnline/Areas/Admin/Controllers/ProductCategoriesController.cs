using System;
using System.Data;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data.Models.Dtos;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ProductCategoriesController : Controller
    {
        IProductCategoriesRepository _category;
        private readonly IMapper _mapper;
        private readonly INotyfService _toastNotification;
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\ProductCategory");
        public ProductCategoriesController(IProductCategoriesRepository productCategoriesRepository, INotyfService toastNotification , IMapper mapper)
        {
            _category = productCategoriesRepository;
            _toastNotification = toastNotification;
            _mapper = mapper;
        }
     
        public async Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            IEnumerable<ProductCategory> items  = await _category.GetAll() ;
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
        public IActionResult Create()
        {
            return View();
        }     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ProductCategoryDto productCategory,IFormFile fileImage)
        {
            if (ModelState.IsValid)
            {               
                if (fileImage != null)
                {
                    productCategory.Icon = Common.Common.SaveFile(path, fileImage);
                }

                productCategory.CreatedDate = DateTime.Now;
                productCategory.ModifiedDate = DateTime.Now;
                productCategory.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
              
                await _category.Add(productCategory);
                _toastNotification.Success("Create Product Category Success");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.Error("Create Product Category Failed");
                return View(productCategory);
            }
        }

        // GET: Admin/ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _category.Get((int)id) == null)
            {
                return NotFound();
            }

            var productCategory =  await _category.Get((int)id);
            var productCategoryDto = _mapper.Map<ProductCategoryDto>(productCategory);
            if (productCategory == null)
            {
                return NotFound();
            }
            return View(productCategoryDto);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCategoryDto productCategory, IFormFile fileImage)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return NotFound();
            }
            ModelState.ClearValidationState("fileImage");
            ModelState.MarkFieldValid("fileImage");
            if (ModelState.IsValid)
            {             
                if (fileImage != null)
                 {
                    productCategory.Icon = Common.Common.SaveFile(path, fileImage);                   
                 }
                productCategory.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                productCategory.ModifiedDate = DateTime.Now;
                productCategory.Title = productCategory.Title;
                productCategory.Description = productCategory.Description;

                await _category.Update(productCategory);
                _toastNotification.Success("Update Product Category Success");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.Error("Update Product Category Failed");
                return View(productCategory);
            }        
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _category.Get(id);
            if (item != null )
            {
                FileInfo file = new FileInfo(path + "\\" + item.Icon);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                bool _delete = await _category.Delete(item);
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
                        var obj = await _category.Get(Convert.ToInt32(item));
                        if (obj != null)
                        {
                            FileInfo file = new FileInfo(path + "\\" + obj.Icon);
                            if (file.Exists)//check file exsit or not  
                            {
                                file.Delete();
                            }
                            await _category.Delete(obj);
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
