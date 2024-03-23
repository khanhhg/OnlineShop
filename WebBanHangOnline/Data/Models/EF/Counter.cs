using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHangOnline.Data.Models.EF
{
    [Table("Counters")]
    public class Counter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CounterId { get; set; }
        public DateTime TimeCount { get; set; }
        public long HitCount { get; set; }
    }
}
