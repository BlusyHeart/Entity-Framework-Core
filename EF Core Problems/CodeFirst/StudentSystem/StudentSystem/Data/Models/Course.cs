using StudentSystem.Data.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            Students = new HashSet<Student>();
            Resources = new HashSet<Resource>();
            Homeworks = new HashSet<Homework>();
        }
        
        public int CourseId { get; set; }

        [Required]
        [MaxLength(GlobalConstans.MaxCourseLength)]
        public string Name { get; set; }
       
        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Resource> Resources { get; set; }
            
        public ICollection<Homework> Homeworks { get; set; }
    }

}
