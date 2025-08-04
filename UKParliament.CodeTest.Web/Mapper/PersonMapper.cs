using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.Mapper;
using UKParliament.CodeTest.Web.ViewModels;
using UKParliament.CodeTest.Services;

namespace UKParliament.CodeTest.Web.Mapper
{
    public class PersonMapper : PersonMapperBase
    {
        private readonly IDepartmentReadService _departmentService;

        public PersonMapper(IDepartmentReadService departmentService)
        {
            _departmentService = departmentService;
        }

        public override PersonViewModel Map(Person person)
        {
            // Lookup department name from database instead of SampleData
            string departmentName = string.Empty;
            try
            {
                var dept = _departmentService.GetByIdAsync(person.DepartmentId).Result;
                if (dept != null)
                {
                    departmentName = dept.Name;
                }
            }
            catch
            {
                // If department lookup fails, leave name empty
                departmentName = string.Empty;
            }
            
            return new PersonViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DOB = person.DOB, // Now using DateOnly directly
                DepartmentId = person.DepartmentId,
                DepartmentName = departmentName,
                FullName = person.FirstName + " " + person.LastName,
                Description = person.Description
            };
        }
    }
}
