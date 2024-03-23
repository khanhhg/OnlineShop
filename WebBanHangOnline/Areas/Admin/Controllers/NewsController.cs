using System;
using System.Data;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class NewsController : Controller
    {
        INewsRepository _newsRepository;
        private readonly INotyfService _toastNotification;
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\News");
        public NewsController(INewsRepository newsRepository, INotyfService toastNotification)
        {
            _newsRepository = newsRepository;
            _toastNotification = toastNotification;
        }
      
        public async  Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            IEnumerable<News> items = await _newsRepository.GetAll();
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x =>  x.Title.Contains(Searchtext)).OrderByDescending(x => x.NewsId);
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
        public async Task<IActionResult> Create(News news, IFormFile fileAvatar)
        {
            if (ModelState.IsValid)
            {
                if (fileAvatar != null)
                {
                    news.Image = Common.Common.SaveFile(path, fileAvatar);
                    news.CreatedDate = DateTime.Now;
                    news.ModifiedDate = DateTime.Now;
                    news.Detail = news.Detail.Replace("../..", String.Empty);
                    news.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    await _newsRepository.Add(news);
                    _toastNotification.Success("Create News Success");               
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.Error("Create News Failed");
                return View(news);
            }         
        }
     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null ||await _newsRepository.Get((int)id) == null)
            {
                return NotFound();
            }
            var news = await _newsRepository.Get((int)id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News news, IFormFile fileAvatar)
        {
            if (id != news.NewsId)
            {
                return NotFound();
            }
            ModelState.ClearValidationState("fileAvatar");
            ModelState.MarkFieldValid("fileAvatar");
            if (ModelState.IsValid)
            {
                {
                    var objNewUpdate = await _newsRepository.Get(id);
                    if (fileAvatar != null)
                    {
                        objNewUpdate.Image = Common.Common.SaveFile(path, fileAvatar);
                    }
                    objNewUpdate.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    objNewUpdate.ModifiedDate = DateTime.Now;
                    objNewUpdate.Title = news.Title;
                    objNewUpdate.Description = news.Description;
                    objNewUpdate.Detail =  news.Detail.Replace("../..", String.Empty);
                    objNewUpdate.Alias = news.Alias;                  
                    objNewUpdate.IsActive = news.IsActive;
                    await _newsRepository.Update(objNewUpdate);
                    _toastNotification.Success("Update News Success");
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                _toastNotification.Error("Update News Failed");
                return View(news);
            }
           
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item =  await _newsRepository.Get(id);
            if (item != null)
            {
                FileInfo file = new FileInfo(path + "\\" + item.Image);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                await _newsRepository.Delete(item);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }          
        }

        [HttpPost]
        public async Task<ActionResult> IsActive(int id)
        {
            var item = await _newsRepository.Get(id);
            if (item != null)
            {
               await _newsRepository.IsActive(item);
                return Json(new { success = true, isAcive = item.IsActive });
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
                        //var obj = _context.News.Find(Convert.ToInt32(item));
                        var obj = await _newsRepository.Get(Convert.ToInt32(item));
                        FileInfo file = new FileInfo(path + "\\" + obj.Image);
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                        await _newsRepository.Delete(obj);                   
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
        public ActionResult TinyMceUpload(IFormFile file)
        {
            string targetFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\news\\details");
            if (file != null)
            {
                var location = "/images/blog/details/" + Common.Common.SaveFile(targetFolder, file);
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
