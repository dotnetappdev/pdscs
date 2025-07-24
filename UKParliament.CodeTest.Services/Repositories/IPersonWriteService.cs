using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web;

namespace UKParliament.CodeTest.Services.Repositories
{
    public interface IPersonWriteService
    {
        Task<ApiResponse<Person>> AddPersonAsync(Person person);
        Task<ApiResponse<Person>> UpdatePersonAsync(int id, Person person);
        Task<ApiResponse<Person>> DeletePersonAsync(int id);
    }
}
