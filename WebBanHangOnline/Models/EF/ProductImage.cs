using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_ProductImage")]
    public class ProductImage
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public string? Image { get; set; }
        public bool? IsDefault { get; set; }

        public virtual Product Product { get; set; }
    }
}
