using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHangOnline.Data.Models.EF
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name can't be empty")]
        [StringLength(150)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email can't be empty")]
        [StringLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Content can't be empty")]
        [StringLength(150)]
        public string Content { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int? Rate { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Product? Product { get; set; }
    }
}
