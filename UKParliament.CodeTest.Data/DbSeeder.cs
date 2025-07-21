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
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { Id = 1, Name = "Sales" },
                    new Department { Id = 2, Name = "Marketing" },
                    new Department { Id = 3, Name = "Finance" },
                    new Department { Id = 4, Name = "HR" });
            }

            if (!context.People.Any())
            {
                context.People.AddRange(
                    new Person { Id = 1, FirstName = "Gaberial", LastName = "Smith", DepartmentId = 1, DOB = new DateOnly(2001, 1, 26) },
                    new Person { Id = 2, FirstName = "David", LastName = "Brown", DepartmentId = 3, DOB = new DateOnly(1977, 6, 26) },
                    new Person { Id = 3, FirstName = "Mike", LastName = "Jones", DepartmentId = 4, DOB = new DateOnly(1985, 3, 20) },
                    new Person { Id = 4, FirstName = "Ricky", LastName = "White", DepartmentId = 1, DOB = new DateOnly(1999, 7, 10) });
            }

            context.SaveChanges();
        }
    }
}
