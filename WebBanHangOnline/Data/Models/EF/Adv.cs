using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBanHangOnline.Data.Models.Common;

namespace WebBanHangOnline.Data.Models.EF
{
    [Table("tb_Adv")]
    public class Adv : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvId { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [StringLength(500)]
        public string? Image { get; set; }
        [StringLength(500)]
        public string? Link { get; set; }
        public int? Type { get; set; }
    }
}
