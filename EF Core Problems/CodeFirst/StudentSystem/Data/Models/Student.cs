using P01_StudentSystem.Data.Commans;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            StudentsCourses = new HashSet<StudentCourse>();
            Homeworks = new HashSet<Homework>();
        }

        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.Student_Name_Max_Length)]
        public string Name { get; set; } = null!;

        [Column(TypeName = "char(10)")]
        public string ?PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; } 

        public DateTime ?Birthday { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; } = null!;

        public ICollection<Homework> Homeworks { get; set; }
    }
}
