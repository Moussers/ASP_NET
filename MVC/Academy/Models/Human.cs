using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Human
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression("^[A-ZА-Я][a-zа-я]+$")]
        [DisplayName("Фамилия")]
        public string last_name { get; set; }
        //Перезаписывает название строки (к примеру: last_name) на то название
        //которое мы указали в скобках после DisplayName на всех страницах где
        //это поле используется это поле.

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("Имя")]
        public string first_name{ get; set; }

        [DisplayName("Отчество")]
        public string? middle_name { get; set; }
        //string? - означает, что поле может быть null 

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Дата рождения")]
        public DateOnly birth_date { get; set; }

        //[EmailAddress]
        //[Required(AllowEmptyStrings = true)]
        [DisplayName("Почта")]
        public string? email { get; set; }
        //[Phone]
        [DisplayName("Номер телефона")]
        public string? phone { get; set; }
        [Column("photo", TypeName = "IMAGE")]
        [DisplayName("Фото")]
        public byte[]? photo { get; set; }

        //          Calculated properties:
        public string FullName 
        {
            get => $"{last_name} {first_name} {middle_name}";
        }
    }
}
