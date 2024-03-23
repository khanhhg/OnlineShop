using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBanHangOnline.Data.Models.Common;

namespace WebBanHangOnline.Data.Models.EF
{
    [Table("tb_Category")]
    public class Category : CommonAbstract
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category name can't be empty")]
        [StringLength(150)]
        public string Title { get; set; }
        public string? Alias { get; set; }
        //[StringLength(150)]
        //public string TypeCode { get; set; }
        public string? Link { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int? Position { get; set; }
    }
}
