namespace Academy.Models
{
    class Student : Human
    {
        public int Group { get; set; }
        public Student
            (int id, string lastName, string firstName, string? middleName,
            string birthDate, string email, string phone, int group
            ) : base(id, lastName, firstName, middleName, birthDate, email, phone) 
        {
            Group = group;
        }
    }
}
