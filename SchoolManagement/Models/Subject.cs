using System.Collections.Generic;

namespace SchoolManagement.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Many-to-many with students
        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
