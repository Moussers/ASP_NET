namespace Academy.Models.ViewModels
{
    public class GroupIndexData
    {
        public IEnumerable<Group> Groups { get; set; }

        public IEnumerable<Student> Students { get; set; }
    }
}
