using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]   //Отключает автоинкремент в таблице
        public int CourseID { get; set; }
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }

        //Navigation properties:
        public ICollection<Enrollment> Enrollments { get; set; } = null;
        public Department Department { get; set; } = null;
        public ICollection<Instructor> Instructors { get; set; } = null;
    }
}
