using System.ComponentModel.DataAnnotations;

namespace WebBanHangOnline.Models
{
	public class OrderViewModel
	{
		[Required(ErrorMessage = "Customer name can't be blank")]
		public string CustomerName { get; set; }
		[Required(ErrorMessage = "Phone number can't be blank")]
		public string Phone { get; set; }
		[Required(ErrorMessage = "Address can't be blank")]
		public string? Address { get; set; }
		public string? Email { get; set; }
		public int TypePayment { get; set; }
	}
}
