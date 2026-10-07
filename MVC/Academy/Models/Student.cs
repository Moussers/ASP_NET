using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Student: Human
    {
        [Key]
        public int stud_id { get; set; }
        
        [Required]
        [ForeignKey(nameof(Group))]
        [DisplayName("Группа")]
        public int group { get; set; }

        //      Navigation properties:
        //[NotMapped]
        public Group? Group { get; set; }
        //? - объект может содержать null-значение
    }
}
