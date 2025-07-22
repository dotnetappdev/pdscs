using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKParliament.CodeTest.Data
{
    public class DbSeeder
    {
        public static void SeedInMemoryData(PersonManagerContext context)
        {
            // Only seed if both tables are empty (startup only)
            if (!context.Departments.Any() && !context.People.Any())
            {
                context.Departments.AddRange(
                    new Department { Id = 1, Name = "Policy" },
                    new Department { Id = 2, Name = "Operations" },
                    new Department { Id = 3, Name = "Digital & Technology" },
                    new Department { Id = 4, Name = "Human Resources" },
                    new Department { Id = 5, Name = "Finance" },
                    new Department { Id = 6, Name = "Communications" },
                    new Department { Id = 7, Name = "Legal Services" }
                );

                // All persons are 18 years old as of today (2025-07-22)
                var dob = DateOnly.Parse("2007-07-22");
                context.People.AddRange(
                    new Person { Id = 1, FirstName = "Alice", LastName = "Johnson", Description = "Policy Analyst", DepartmentId = 1, DOB = dob },
                    new Person { Id = 2, FirstName = "Ben", LastName = "Williams", Description = "Operations Coordinator", DepartmentId = 2, DOB = dob },
                    new Person { Id = 3, FirstName = "Chloe", LastName = "Evans", Description = "Digital Apprentice", DepartmentId = 3, DOB = dob },
                    new Person { Id = 4, FirstName = "Daniel", LastName = "Green", Description = "HR Assistant", DepartmentId = 4, DOB = dob },
                    new Person { Id = 5, FirstName = "Ella", LastName = "Brown", Description = "Finance Trainee", DepartmentId = 5, DOB = dob },
                    new Person { Id = 6, FirstName = "Freddie", LastName = "Taylor", Description = "Communications Intern", DepartmentId = 6, DOB = dob },
                    new Person { Id = 7, FirstName = "Grace", LastName = "Morgan", Description = "Legal Assistant", DepartmentId = 7, DOB = dob }
                );

                context.SaveChanges();
            }
        }
    }
}
