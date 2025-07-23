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
            if (!context.Departments.Any() && !context.People.Any())
            {
                context.Departments.AddRange(SampleData.GetDepartments());
                context.People.AddRange(SampleData.GetPeople());
                context.SaveChanges();
            }
        }
    }
}
