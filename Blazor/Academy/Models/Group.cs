namespace Academy.Models
{
    public class Group
    {
        public int Id { get; set; } 
        public string GroupName { get; set; }
        public string TypeLearning { get; set; }
        public int LearningDays { get; set; }
        public DateTime DateStart { get; set; }
    }
}
