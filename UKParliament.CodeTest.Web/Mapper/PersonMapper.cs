using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.Mapper;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mapper
{
    public class PersonMapper : PersonMapperBase
    {
        public override PersonViewModel Map(Person person)
        {
            // Lookup department name from static list or context
            string departmentName = string.Empty;
            switch (person.DepartmentId)
            {
                case 1: departmentName = "Sales"; break;
                case 2: departmentName = "Marketing"; break;
                case 3: departmentName = "Finance"; break;
                case 4: departmentName = "HR"; break;
                default: departmentName = ""; break;
            }
            return new PersonViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                dob = person.DOB,
                DepartmentId = person.DepartmentId,
                DepartmentName = departmentName,
                FullName = person.FirstName + " " + person.LastName,
                Description = person.Description
            };
        }
    }
}
