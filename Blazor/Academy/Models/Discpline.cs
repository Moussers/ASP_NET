using System.ComponentModel.DataAnnotations;

namespace Academy.Models
{
    public class Discipline
    {
        [Key]
        public short discipline_id { get; set; }
        public string? discipline_name { get; set; }
        public byte number_of_lessons { get; set; }
    }
}
