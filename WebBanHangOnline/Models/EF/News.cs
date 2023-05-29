using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBanHangOnline.Models.Common;
using System.Web.Mvc;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_News")]
    public class News : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NewsId { get; set; }
        [Required(ErrorMessage = "Title can't be emptyn")]
        [StringLength(150)]
        public string Title { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        [AllowHtml]
        public string? Detail { get; set; }
        public string? Image { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }
        public bool? IsActive { get; set; }
        public Category Category { get; set; }
    }
}
