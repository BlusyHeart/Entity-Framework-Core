using Microsoft.EntityFrameworkCore;
using StudentSystem.Data.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Data.Models
{
    public class Student
    {

        public Student()
        {
            Courses = new HashSet<Course>();
            Homework = new HashSet<Homework>();
        }

        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(GlobalConstans.MaxNameLength)]      
        public string Name { get; set; }

        [Column(TypeName = "char(10)")]
        public string PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public ICollection<Course> Courses { get; set; }

        public ICollection<Homework> Homework { get; set; }

        
    }

}
