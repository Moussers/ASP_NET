using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Directions
    {
        [Key]
        [Column(TypeName = "TINYINT")]
        public int direction_id { get; set; }
        public string direction_name { get; set; }
    }
}
