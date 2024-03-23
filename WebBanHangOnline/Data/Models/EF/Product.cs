using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBanHangOnline.Data.Models.Common;

namespace WebBanHangOnline.Data.Models.EF
{
    [Table("tb_Product")]
    public class Product : CommonAbstract
    {
        public Product()
        {
            ProductImages = new HashSet<ProductImage>();           
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(250)]
        public string? Alias { get; set; }

        [StringLength(50)]
        public string? ProductCode { get; set; }
        public string? Description { get; set; }
        [DisplayFormat(HtmlEncode = true)]
        public string? Detail { get; set; }

        [StringLength(250)]
        public string? Image { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal OriginalPrice { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal PriceSale { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public int? ViewCount { get; set; }
        public bool IsHome { get; set; } = false;
        public bool IsSale { get; set; } = false;
        public bool IsFeature { get; set; } = false;
        public bool IsHot { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
