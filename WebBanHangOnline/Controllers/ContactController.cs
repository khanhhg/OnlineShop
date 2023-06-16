using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ContactController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly INotyfService _toastNotification;
		public ContactController(ApplicationDbContext context, INotyfService toastNotification)
		{
			_context = context;
			_toastNotification = toastNotification;
		}
		public IActionResult Index()
        {
            return View();
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Contact contact)
		{
			if (ModelState.IsValid)
			{
				contact.CreatedDate = DateTime.Now;
				contact.ModifiedDate = DateTime.Now;
				contact.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
				contact.IsRead = false;
				_context.Contact.Add(contact);
				await _context.SaveChangesAsync();
				_toastNotification.Success("Send message success");

				return RedirectToAction(nameof(Index));
			}
			else
			{
				_toastNotification.Error("Send message error");
				
				return View(contact);
			}
		}
	}
}
