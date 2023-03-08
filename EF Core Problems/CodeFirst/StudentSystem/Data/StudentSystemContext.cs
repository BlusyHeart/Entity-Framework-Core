using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Commans;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
            
        }

        public StudentSystemContext(DbContextOptions contextOptions)
        {
            ContextOptions = contextOptions;
        }

        public DbSet<Course> Courses { get; set; } 

        public DbSet<Homework> Homeworks { get; set; } 

        public DbSet<Resource> Resources { get; set; } 

        public DbSet<Student> Students { get; set; } 

        public DbSet<StudentCourse> StudentsCourses { get; set; }
        public DbContextOptions ContextOptions { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GlobalConstants.Connection_String);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sp => new
                {
                    sp.StudentId, sp.CourseId
                });
        }
    }
}
