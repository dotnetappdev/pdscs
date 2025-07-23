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
            // Lookup department name from SampleData
            string departmentName = string.Empty;
            var departments = UKParliament.CodeTest.Data.SampleData.GetDepartments();
            var dept = departments.FirstOrDefault(d => d.Id == person.DepartmentId);
            if (dept != null)
            {
                departmentName = dept.Name;
            }
            return new PersonViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DOB = person.DOB.HasValue ? person.DOB.Value.ToString("yyyy-MM-dd") : string.Empty,
                DepartmentId = person.DepartmentId,
                DepartmentName = departmentName,
                FullName = person.FirstName + " " + person.LastName,
                Description = person.Description
            };
        }
    }
}
