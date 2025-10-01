using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for StudentSubject
            modelBuilder.Entity<StudentSubject>()
                .HasKey(ss => new { ss.StudentId, ss.SubjectId });

            // Relations for StudentSubject
            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.StudentId);

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectId);

            // SchoolClass -> School
            modelBuilder.Entity<SchoolClass>()
                .HasOne(sc => sc.School)
                .WithMany(s => s.SchoolClasses)
                .HasForeignKey(sc => sc.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            // Student -> School
            modelBuilder.Entity<Student>()
                .HasOne(s => s.School)
                .WithMany()
                .HasForeignKey(s => s.SchoolId)
                .OnDelete(DeleteBehavior.Restrict); // prevents multiple cascade paths

            // Student -> SchoolClass
            modelBuilder.Entity<Student>()
                .HasOne(s => s.SchoolClass)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.SchoolClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
