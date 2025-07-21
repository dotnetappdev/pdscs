using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;

namespace UKParliament.CodeTest.Services
{
    public class PersonReadService
    {
        private readonly IReadOnlyRepository<Person> _readRepository;

        public PersonReadService(IReadOnlyRepository<Person> readRepository)
        {
            _readRepository = readRepository;
        }

        public Person GetPerson(int id) => _readRepository.GetById(id);
        public IEnumerable<Person> GetAllPersons() => _readRepository.GetAll();
    }

}

