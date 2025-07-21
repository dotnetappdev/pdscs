using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.Mapper;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Services.Mapper
{
    public class PersonMapper : PersonMapperBase
    {
        public override PersonViewModel Map(Person person) => new PersonViewModel
        {
            FullName = person.FirstName + " " + person.LastName,
            DOB = person.DOB
        };

    }
}
