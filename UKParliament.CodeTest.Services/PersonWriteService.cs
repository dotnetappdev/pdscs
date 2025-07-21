using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;

namespace UKParliament.CodeTest.Services
{
    public class PersonWriteService
    {
        private readonly IWriteOnlyRepository<Person> _writeRepository;

        public PersonWriteService(IWriteOnlyRepository<Person> writeRepository)
        {
            _writeRepository = writeRepository;
        }

        public void SavePerson(Person person) => _writeRepository.Save(person);
        public void DeletePerson(int id) => _writeRepository.Delete(id);
    }
}
