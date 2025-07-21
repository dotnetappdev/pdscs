using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Repositories
{
    public interface IPersonWriteService<T>
    {
        void SavePerson(Person person);

        void AddPerson(Person person);
        Task<Person> UpdatePerson(Person person);
        Task DeletePerson(int id);
    }
}
