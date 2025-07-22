using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Services.Repositories;

namespace UKParliament.CodeTest.Data.Repositories
{
    public class PersonReadRepository : IPersonReadService<Person>
    {
        private readonly PersonManagerContext _context;

        public PersonReadRepository(PersonManagerContext context) => _context = context;

        public Person GetById(int id) => _context.People.Find(id);
        public IEnumerable<Person> GetAll()
        {
            var test = _context.People.ToList();
            Console.WriteLine($"Repository GetAll: {test.Count} people found");
            return test;
        }
    }
}
