using System.Collections.Generic;

namespace SchoolManagement.Models
{
    public class SchoolClass
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Foreign Key
        public int SchoolId { get; set; }

        // Navigation property (nullable for binding)
        public School? School { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
