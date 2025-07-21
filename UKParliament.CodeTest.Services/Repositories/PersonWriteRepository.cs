using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Repositories
{
    public class PersonWriteRepository : IPersonWriteService<Person>
    {
        private readonly PersonManagerContext _context;
        public PersonWriteRepository(PersonManagerContext context) => _context = context;


        public void SavePerson(Person person)
        {
            _context.People.Add(person);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var person = _context.People.Find(id);
            if (person != null)
            {
                _context.People.Remove(person);
                _context.SaveChanges();
            }
        }

        public void AddPerson(Person person)
        {
            _context.People.Add(person);
            _context.SaveChanges();
        }

        public Task<Person> UpdatePerson(Person person)
        {
            throw new NotImplementedException();
        }

        public Task DeletePerson(int id)
        {
            throw new NotImplementedException();
        }
    }
}
