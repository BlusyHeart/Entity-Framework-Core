using Microsoft.EntityFrameworkCore;
using StudentSystem.Data.Commons;
using StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {

        public StudentSystemContext()
        {

        }

        public DbSet<Course> Course { get; set; }

        public DbSet <Homework> Homeworks { get; set; }

        public DbSet <Resource> Resources { get; set; }

        public DbSet <Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }



    }
}
