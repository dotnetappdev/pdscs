using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;

namespace UKParliament.CodeTest.Services
{
    public class PersonReadService : IPersonReadService<Person>
    {
        private readonly IPersonReadService<Person> _readRepository;

        public PersonReadService(IPersonReadService<Person> readRepository)
        {
            _readRepository = readRepository;
        }

        public Person GetPerson(int id) => _readRepository.GetById(id);
        public IEnumerable<Person> GetAllPersons() //=>
        {
            var test = _readRepository.GetAll();
            return test;
        }

        public Person GetById(int id)
        {
            return (_readRepository.GetById(id));
        }


        public IEnumerable<Person> GetAll() => _readRepository.GetAll();
    }


}

