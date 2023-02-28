using SoftUni.Data;
using SoftUni.Models;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace SoftUni.Services
{
    public static class DbServices
    {        
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    employee = $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}"

                }).ToArray();

            return string.Join(Environment.NewLine, employees.Select(e => e.employee));

        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
           int targetSalaray = 50000;

            var employees = context.Employees
                 .Where(e => e.Salary > targetSalaray)
                 .OrderBy(e => e.FirstName)
                 .Select(e => new
                 {
                     Output = $"{e.FirstName} - {e.Salary:f2}"
                 }).ToArray();

            return string.Join(Environment.NewLine, employees.Select(e => e.Output));
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    Output = $"{e.FirstName} {e.LastName} {e.Department} ${e.Salary:f2}"

                }).ToArray();

            return string.Join(Environment.NewLine, employees.Select(e => e.Output));
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
           Address address = new()
           {
                AddressText = "Vitoshka15",
                TownId = 4
           };

           Employee? employee = context.Employees                
                .FirstOrDefault(e => e.LastName == "Nakov");

           employee!.Address = address;

           context.SaveChanges();

           var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToArray();

            return string.Join(Environment.NewLine, employees);
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new ();

            var employee = context.Employees
                .Where(e => e.EmployeesProjects.Any(ep => ep.Project!.StartDate.Year >= 2001 &&
                                                          ep.Project!.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                   EmployeeInfo = $"{e.FirstName}{e.LastName}{e.Manager!.FirstName}{e.Manager.LastName}",
                   Projects = e.EmployeesProjects
                    .Select(ep => new
                    {
                        ProjectName = ep.Project!.Name,
                        StartDate = ep.Project!.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                        EndDate = ep.Project!.EndDate.HasValue ?
                            ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished"
                    }).ToArray()
                }).ToArray();

            foreach (var e in employee)
            {
                sb.AppendLine(e.EmployeeInfo);

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town!.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    Output = $"{a.AddressText}, {a.Town!.Name} - {a.Employees.Count} employees"

                }).ToArray();

            return string.Join(Environment.NewLine, addresses.Select(a => a.Output));
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new ();

            var employees = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    EmployeeInfo = $"{e.FirstName} {e.LastName} - {e.JobTitle}",
                    Projects = e.EmployeesProjects
                        .Select(p => new
                        {
                            ProjectName = p.Project!.Name

                        }).OrderBy(p => p.ProjectName)
                            .ToArray()
                }).ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine(e.EmployeeInfo);

                foreach (var p in e.Projects)
                {
                    sb.AppendLine(p.ProjectName);
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new ();

            var department = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentInfo = $"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}",
                    Employess = d.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle

                    }).OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToArray()

                }).ToArray();

            foreach (var d in department)
            {
                sb.AppendLine(d.DepartmentInfo);

                foreach (var e in d.Employess)
                {
                    sb.AppendLine($"{e.FirstName}{e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new ();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Description)
                .ThenBy(p => p.StartDate)
                .Select(pj => new
                {
                    pj.Name,
                    pj.Description,
                    StartDate = pj.StartDate.ToString("M/d/yyyy h:mm:ss tt"), 

                }).ToArray();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.Name}");
                sb.AppendLine($"{p.Description}");
                sb.AppendLine($"{p.StartDate}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder output = new ();

            var departments = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employess = context.Employees
                .Where(e => departments.Contains(e.Department.Name))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var e in employess)
            {
                e.Salary *= 1.12M;             
            }

            context.SaveChanges();

            foreach (var e in employess)
            {
                output.AppendLine($"{e.FirstName}{e.LastName} (${e.Salary:f2})");
            }

            return output.ToString().TrimEnd();          
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employess = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    Output = $"{e.FirstName} - {e.LastName} - ${e.Salary:f2}"

                }).ToArray();

            return string.Join(Environment.NewLine, employess.Select(e => e.Output));
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            var employeeProject = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeeProject!);

            context.SaveChanges();

            context.Projects.Remove(project!);

            context.SaveChanges();

            var projects = context.Projects
                .Select(p => new
                {
                    p.Name
                }).ToArray();

            return string.Join(Environment.NewLine, projects.Select(p => p.Name));
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            var employee = context.Employees
                .Where(e => e.Address!.TownId == town!.TownId);

            foreach (var e in employee)
            {
                e.Address = null;
            }           

            var addresses = context.Addresses
                .Where(a => a.TownId == town!.TownId);

            int count = 0;
            foreach (var a in addresses)
            {
                count++;
                context.Addresses.Remove(a);
            }
          
            context.Towns.Remove(town!);

            context.SaveChanges();

            return $"{count} addresses in Seattle were deleted";
                
        }
    }
}
