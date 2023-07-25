using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBanHangOnline.Models.EF
{
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string CustomerName { get; set; }
        [StringLength(500)]
        public string PhoneNumber { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        public string UserID { get; set; }
    }
}
