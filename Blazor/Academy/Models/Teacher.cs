using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Teacher: Human
    {
        [Key]
        [Column(TypeName = "SMALLINT")]
        public short teacher_id { get; set; }
        //smallint - short
        public DateTime? work_since { get; set; }
        public decimal? rate { get; set; }
        //smallmoney - decimal
    }
}
