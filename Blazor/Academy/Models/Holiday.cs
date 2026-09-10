using System.ComponentModel.DataAnnotations;

namespace Academy.Models
{
    public class Holiday
    {
        [Key]
        public byte holiday_id {get;set;}
        public string holiday_name {get;set;}
        public byte duration { get;set;}
        public byte? month { get;set;}
        public byte? day { get;set;}
    }
}
