using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Discipline
    {
        [Key]
        [Column(TypeName = "SMALLINT")]
        public int discipline_id { get; set; }
        //smallint - short

        [Required]
        [DisplayName("Название дисциплины")]
        public string discipline_name { get; set; }

        [Required]
        [Column(TypeName = "TINYINT")]
        [DisplayName("Количество занятий")]
        public int number_of_lessons { get; set; }
        //tinyint - byte

        //Navigation properties:
        public ICollection<TeachersDisciplinesRelation> TeachersRelations { get; set; }
        //ссылка на связывающий класс TeachersDisciplinesRelation, который связывает
        //классы teacher и discipline
        //работает по принципу метода JOIN в SQL

    }
}
