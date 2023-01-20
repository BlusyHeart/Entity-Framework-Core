using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SoftUni
{
    public class Program
    {
        static void Main()
        {
            var context = new SoftUniContext();

            Stopwatch stopwatch= Stopwatch.StartNew();

            RemoveTown(context);
            
            //(context);

            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        private static void RemoveTown(SoftUniContext context)
        {
            var town = context.Towns.FirstOrDefault(t => t.Name == "Nevada");

            context.Towns.Remove(town);

            context.SaveChanges();
            
        }

        private static string RemoveAddress(SoftUniContext context)
        {
            var address = context
                .Addresses
                .Where(a => a.Town.Name == "Nevada").ToArray();
                       
            foreach (var a in address)
            {
                a.TownId = null;
            }

            context.SaveChanges();

            return address.Length.ToString();

        }

        public static void  RemoveAddresFromEmployee(SoftUniContext context)
        {
            var employyes = context
                .Employees
                .Where(e => e.Address.Town.Name == "Nevada")                
                .ToArray();

            foreach (var e in employyes)
            {
                e.AddressId = null;
            }
                       
            context.SaveChanges();
           
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary

                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var e in employees)
            {
                output.AppendLine($"{e.FirstName} - {e.LastName} - (${e.Salary:f2})");
            }

            return output.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {

            StringBuilder output = new StringBuilder();

            var departments = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employees = context
                .Employees
                .Where(e => departments.Contains(e.Department.Name))               
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var e in employees)
            {
                e.Salary *= 1.12M;
            }
                    
            foreach (var e in employees)
            {
                output.AppendLine($"{e.FirstName}{e.LastName} ({e.Salary:f2})");
            }


            return output.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();

            var projects = context
                .Projects                
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                }).OrderBy(p => p.Name).ToArray();

            foreach (var p in projects)
            {
                output.AppendLine($"{p.Name}");
                output.AppendLine(p.Description);
                output.AppendLine(p.StartDate);
            }

            return output.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();

            var departments = context
                .Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerFistName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    AllEmployess = d.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,

                    }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToArray()

                }).ToArray();

            foreach (var d in departments)
            {
                output.AppendLine($"{d.Name} - {d.ManagerFistName} {d.ManagerLastName} - Operational Manager");
                foreach (var e in d.AllEmployess)
                {
                    output.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }
            return output.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();

            var employee = context
                .Employees    
                .Where(e => e.EmployeeId == 1)
                .Select(e => new
                {
                    
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    AllProjects = e.Projects.Select(p => new
                    {
                        ProjectName = p.Name

                    }).OrderBy(e => e.ProjectName).ToArray()

                }).FirstOrDefault();
            
            output.AppendLine($"{employee.FirstName}, {employee.LastName} - {employee.JobTitle} employees");

            foreach (var project in employee.AllProjects)
            {
                output.AppendLine($"{project.ProjectName}");
            }
            

            return output.ToString().TrimEnd();

        }

        public static string GetAddressesByTown(SoftUniContext context)
        {

            StringBuilder output = new StringBuilder();

            var addresses = context
                .Addresses                                                
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count

                })
                .OrderByDescending(a => a.EmployeeCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)                 
                .ToArray();

            foreach (var a in addresses)
            {
                output.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return output.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();

            var emloyees = context
                .Employees
                .Where(e => e.Projects.Any(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    AllProjects = e.Projects
                    .Select(ep => new
                    {
                        ProjectName = ep.Name,
                        StartDate = ep.StartDate
                        .ToString("M/d/yyyy h:mm:ss tt"),
                        EndDate = ep.EndDate.HasValue ?
                        ep.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished"

                    }).ToArray()

                }).ToArray();
            
            foreach (var e in emloyees)
            {
                output.AppendLine($"{e.FirstName} {e.LastName} - {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var project in e.AllProjects)
                {
                    output.AppendLine($"--{project.ProjectName} - {project.StartDate} - {project.EndDate}");
                }
            }

            return output.ToString().TrimEnd();
        }

        public static string GetTop10Address(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();

            var employees = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToArray();

            foreach (var address in employees)
            {
                output.AppendLine(address);
            }

            return output.ToString().TrimEnd();
        }

        public static void AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4,
            };

            context.Addresses.Add(address);

            var employee = context.
                Employees.
                First(e => e.LastName == "Nakov");

            employee.Address = address;
            context.SaveChanges();
        }
    }
}
