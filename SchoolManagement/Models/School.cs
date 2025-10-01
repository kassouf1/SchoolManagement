namespace SchoolManagement.Models
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<SchoolClass> SchoolClasses { get; set; } = new List<SchoolClass>();
    }
}
