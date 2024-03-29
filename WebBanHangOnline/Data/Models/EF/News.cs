﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBanHangOnline.Data.Models.Common;

namespace WebBanHangOnline.Data.Models.EF
{
    [Table("tb_News")]
    public class News : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsId { get; set; }
        [Required(ErrorMessage = "Title can't be empty")]
        [StringLength(150)]
        public string Title { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        [DisplayFormat(HtmlEncode = true)]
        public string? Detail { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
    }
}
