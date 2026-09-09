namespace Academy.Models
{
    class Human
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Human
            (
            int id, string lastName, string firstName, string? middleName, 
            string birthDate, string email, string phone
            )
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            BirthDate = birthDate;
            Email = email;
            Phone = phone;
        }
    }
}
