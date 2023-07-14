using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\News");
        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/News
        public  IActionResult Index(string Searchtext, int? page = 1)
        {
            IEnumerable<News> items = _context.News.OrderByDescending(x => x.NewsId);
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x =>  x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        // GET: Admin/News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: Admin/News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news, IFormFile fileAvatar)
        {
            if (ModelState.IsValid)
            {
                if (fileAvatar.FileName != null)
                {                 
                    FileInfo fileInfo = new FileInfo(fileAvatar.FileName);

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
                            fileAvatar.CopyTo(stream);
                        }
                        news.Image = filename;
                    }
                    news.CreatedDate = DateTime.Now;
                    news.ModifiedDate = DateTime.Now;
                    news.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _context.News.Add(news);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                try
                {
                    var objNewUpdate = _context.News.Where(x => x.NewsId == id).FirstOrDefault();
                    if (fileAvatar != null)
                    {                  
                        FileInfo fileInfo = new FileInfo(fileAvatar.FileName);

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
                                fileAvatar.CopyTo(stream);
                            }
                            objNewUpdate.Image = filename;
                        }
                    }
                    objNewUpdate.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    objNewUpdate.ModifiedDate = DateTime.Now;
                    objNewUpdate.Title = news.Title;
                    objNewUpdate.Description = news.Description;
                    objNewUpdate.Detail = news.Detail;
                    objNewUpdate.Alias =  news.Alias;
                    objNewUpdate.SeoDescription = news.SeoDescription;  
                    objNewUpdate.SeoKeywords = news.SeoKeywords;
                    objNewUpdate.SeoTitle = news.SeoTitle;
                    objNewUpdate.IsActive = news.IsActive;
                    _context.Update(objNewUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.NewsId))
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
            return View(news);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _context.News.Find(id);
            if (item != null)
            {
                FileInfo file = new FileInfo(path + "\\" + item.Image);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                _context.News.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _context.News.Find(id);
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
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {                       
                        var obj = _context.News.Find(Convert.ToInt32(item));
                        FileInfo file = new FileInfo(path + "\\" + obj.Image);
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                        _context.News.Remove(obj);
                        _context.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        private bool NewsExists(int id)
        {
          return (_context.News?.Any(e => e.NewsId == id)).GetValueOrDefault();
        }
    }
}
