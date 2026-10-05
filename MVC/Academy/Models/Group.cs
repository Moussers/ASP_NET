using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Group
    {
        [Key]
        public int group_id { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 5)]
        [Column(TypeName = "NCHAR(10)")]
        [DisplayName("Название группы")]
        public string group_name { get; set; }
        [Required]
        [Column(TypeName = "TINYINT")]
        [ForeignKey(nameof(Direction))]
        [DisplayName("Направление")]
        public int direction { get; set; }
        //tinyint - byte
        [DisplayName("Учебные дни")]
        [Column("weekdays", TypeName = "TINYINT")]
        public int? learning_days { get; set; } = 0;
        [DisplayName("Время")]
        public TimeOnly? start_time { get; set; }
        [DisplayName("Дата")]
        public DateOnly? start_date { get; set; }

        //Navigation properties - это переменные, точнее свойства которые хранят данные из связанных таблиц.
        //То есть у нас не только хранится сама запись, а данные на которрые эта запись ссылается в других
        //таблицах.

        //          Navigation properties:
        public Direction Direction { get; set; } = default!;
        public ICollection<Student> Students { get; set; } = default!;
        //ICollection<Student> - массив из объектов класса студент
    }
}
