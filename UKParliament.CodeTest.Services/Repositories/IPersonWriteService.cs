using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web;

namespace UKParliament.CodeTest.Services.Repositories
{
    public interface IPersonWriteService<T>
    {
        void SavePerson(Person person);


        ApiResponse<Person> AddPerson(Person person);
        ApiResponse<Person> UpdatePerson(int id, Person person);
        Task<ApiResponse<Person>> DeletePerson(int id);

    }
}
