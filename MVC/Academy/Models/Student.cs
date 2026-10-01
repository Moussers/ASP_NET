using System.ComponentModel.DataAnnotations;

namespace Academy.Models
{
    public class Student: Human
    {
        [Key]
        public int stud_id { get; set; }
        public int? group { get; set; }
    }
}
