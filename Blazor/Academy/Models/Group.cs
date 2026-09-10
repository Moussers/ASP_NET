using System.ComponentModel.DataAnnotations;

namespace Academy.Models
{
    public class Group
    {
        [Key]
        public int group_id { get; set; } 
        public string? group_name { get; set; }
        public byte? direction { get; set; }
        //tinyint - byte
        public byte? weekdays { get; set; }
        public TimeOnly? start_time { get; set; }
        public DateTime? start_date { get; set; }
    }
}
