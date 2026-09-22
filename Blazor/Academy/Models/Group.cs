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
        public string group_name { get; set; }
        [Required]
        [Column(TypeName = "TINYINT")]
        [ForeignKey(nameof(Direction))]
        public int direction { get; set; }
        //tinyint - byte
        [Column("weekdays", TypeName = "TINYINT")]
        public int? learning_days { get; set; } = 0;
        public TimeOnly? start_time { get; set; }
        public DateOnly? start_date { get; set; }

        //Navigation properties - это переменные, точнее свойства которые хранят данные из связанных таблиц.
        //То есть у нас не только хранится сама запись, а данные на которрые эта запись ссылается в других
        //таблицах.

        //          Navigation properties:
        public Direction Direction { get; set; } = default!;
        ICollection<Student> Students { get; set; } = default!;
    }
}
