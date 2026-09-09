namespace Academy.Models
{
    class Teacher: Human
    {
        public string WorkSince { get; set; }
        public Teacher
            (int id, string lastName, string firstName, string middleName, string birthDate, 
            string email, string phone, string workSince
            ) : base(id, lastName, firstName, middleName, birthDate, email, phone)
        {
            WorkSince = workSince;
        }
    }
}
