using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mapper
{
    public abstract class PersonMapperBase
    {
        public abstract PersonViewModel Map(Person person);

        public IEnumerable<PersonViewModel> MapList(IEnumerable<Person> people)
        {
            return people.Select(Map);
        }
    }
}
