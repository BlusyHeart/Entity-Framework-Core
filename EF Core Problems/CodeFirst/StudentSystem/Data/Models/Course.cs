using P01_StudentSystem.Data.Commans;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            StudentsCourses = new HashSet<StudentCourse>();
            Resources = new HashSet<Resource>();
            Homeworks = new HashSet<Homework>();
        }

        [Key]
        public int CourseId { get; set; }

        [MaxLength(GlobalConstants.Course_Name_Max_Length)]
        [Required]
        public string Name { get; set; } = null!;
      
        public string ?Description { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; } 

        public ICollection<StudentCourse> StudentsCourses { get; set; } = null!;

        public ICollection <Resource> Resources { get; set; }

        public ICollection <Homework> Homeworks { get; set; }
    }
}
