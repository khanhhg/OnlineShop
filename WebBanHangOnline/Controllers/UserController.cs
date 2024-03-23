using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Controllers
{
    public class UserController : Controller
    {
        //private readonly ApplicationDbContext _context;
        IUserRepository _IUser;
        public UserController(IUserRepository userRepository)
        {
          
            _IUser = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            string vUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = await _IUser.Get(vUserID);
            ViewBag.UserID = vUserID;
            return View(item);
        }
        public async Task<IActionResult> AddOrUpdate()
        {
            string vUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserID = vUserID;
            var item = await _IUser.Get(vUserID);
            if (item ==null)
            {
                return View();
            }
            else
            {             
                return View(item);
            }         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdate(UserProfile userProfile)
        {
            ModelState.ClearValidationState("Id");
            ModelState.MarkFieldValid("Id");
            if (ModelState.IsValid)
            {
                var item = await _IUser.Get(userProfile.UserID);
                if (item == null)
                {
                    await _IUser.Add(userProfile);
                }
                else
                {
                    await _IUser.Update(userProfile);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(userProfile);
            }
        }

    }
}
