using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mapper
{
    public interface IPersonMapper
    {
        PersonViewModel Map(Person person);
        IEnumerable<PersonViewModel> MapList(IEnumerable<Person> people);
    }
}
