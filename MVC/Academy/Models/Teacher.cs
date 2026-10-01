using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Academy.Models
{
    public class Teacher: Human
    {
        [Key]
        public short teacher_id { get; set; }
        public DateOnly work_since { get; set; }
        public decimal rate { get; set; }
    }
}