using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBanHangOnline.Data.Models.Common;

namespace WebBanHangOnline.Data.Models.EF
{
    [Table("tb_Order")]
    public class Order : CommonAbstract
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required(ErrorMessage = "CustomerName can't be empty")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Phone can't be empty")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address can't be empty")]
        public string Address { get; set; }
        public string? Email { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public int TypePayment { get; set; }
        public string UserID { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
