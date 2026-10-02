using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Student: Human
    {
        [Key]
        [Column("stud_id")]
        [Required]
        public int stud_ID { get; set; }
        public int? group { get; set; }
    }
}
