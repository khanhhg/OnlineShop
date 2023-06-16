﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebBanHangOnline.Models.Common;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Posts")]
    public class Posts : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PostsId { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(150)]
        public string? Alias { get; set; }
        public string? Description { get; set; }

        [AllowHtml]
        public string? Detail { get; set; }

        [StringLength(250)]
        public string? Image { get; set; }

        [StringLength(250)]
        public string? SeoTitle { get; set; }
        [StringLength(500)]
        public string? SeoDescription { get; set; }
        [StringLength(250)]
        public string? SeoKeywords { get; set; }
        public bool? IsActive { get; set; }
        public virtual Category Category { get; set; }
       
    }
}