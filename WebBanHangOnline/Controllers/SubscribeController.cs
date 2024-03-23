using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class SubscribeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SubscribeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult Create(Subscribe req)
        {
            if (ModelState.IsValid)
            {
                _context.Subscribe.Add(new Subscribe { Email = req.Email, CreatedDate = DateTime.Now });
                _context.SaveChanges();
                return Json(new { Success = true });
            }
            return Json(new { success = false });
        }
    }
}
