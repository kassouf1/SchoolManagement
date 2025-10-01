using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public int SchoolId { get; set; }
        public int SchoolClassId { get; set; }

        // Navigation properties
        public School School { get; set; }
        public SchoolClass SchoolClass { get; set; }
        public ICollection<StudentSubject> StudentSubjects { get; set; }

        // This is the fix: non-mapped property for form binding
        [NotMapped]
        public int[] SelectedSubjectIds { get; set; }
    }
}
