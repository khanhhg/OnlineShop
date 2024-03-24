
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHangOnline.Data.Models.EF
{
    [Table("tb_OrderDetail")]
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
}
