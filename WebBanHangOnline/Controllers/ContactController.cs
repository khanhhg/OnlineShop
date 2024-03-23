using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Controllers
{
    public class ContactController : Controller
    {		
		private readonly INotyfService _toastNotification;
		IContactRepository _contactRepository;
		public ContactController(IContactRepository contactRepository, INotyfService toastNotification)
		{
            _contactRepository = contactRepository;
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
				await _contactRepository.Add(contact);
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
