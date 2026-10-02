using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Academy.Models
{
    public class Teacher: Human
    {
        [Key]
        [Column("teacher_id")]
        [Required]
        public short teacher_ID { get; set; }
        public DateOnly work_since { get; set; }
        public decimal rate { get; set; }
    }
}