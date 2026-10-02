using Academy.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Group
    {
        [Key]
        [Column("group_id")]
        [Required]
        public int group_ID { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string? group_name { get; set; }
        public byte direction { get; set; }
        public byte? weekdays { get; set; }
        public TimeOnly? start_time { get; set; }
        public DateOnly? start_date { get; set; }
    }
}
