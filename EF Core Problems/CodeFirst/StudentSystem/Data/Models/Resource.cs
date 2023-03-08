using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Commans;
using P01_StudentSystem.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [MaxLength(GlobalConstants.Resource_Name_Max_Length)]
        public string Name { get; set; } = null!;

        [Unicode(false)]
        public string Url { get; set; } = null!;

        public ResourceType ResourceType { get; set;}

        [ForeignKey(nameof(Course))]
        [Required]
        public int CourseId { get; set; }

        public Course ?Course { get; set; }
    }
}
