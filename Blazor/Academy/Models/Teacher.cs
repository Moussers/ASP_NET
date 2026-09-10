using System.ComponentModel.DataAnnotations;

namespace Academy.Models
{
    public class Teacher
    {
        [Key]
        public short teacher_id { get; set; }
        //smallint - short
        public string? last_name { get; set; }
        public string? first_name { get; set; }
        public string? middle_name { get; set; }
        public DateTime? birth_date { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public DateTime? work_since { get; set; }
        public decimal? rate { get; set; }
        //smallmoney - decimal
    }
}
