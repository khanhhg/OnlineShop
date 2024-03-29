﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebBanHangOnline.Data.Models.Common;

namespace WebBanHangOnline.Data.Models.EF
{
    [Table("tb_Contact")]
    public class Contact : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Name cannot be null")]
        [StringLength(150, ErrorMessage = "Must not exceed 150 characters")]
        public string Name { get; set; }
        [StringLength(150, ErrorMessage = "Must not exceed 150 characters")]
        public string Email { get; set; }
        public string? Website { get; set; }
        [StringLength(4000)]
        public string? Message { get; set; }
        public bool? IsRead { get; set; }
    }
}
