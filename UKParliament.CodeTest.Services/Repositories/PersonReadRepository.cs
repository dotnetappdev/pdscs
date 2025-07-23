using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using UKParliament.CodeTest.Services.Repositories;

namespace UKParliament.CodeTest.Data.Repositories
{
    public class PersonReadRepository : IPersonReadService<Person>
    {
        private readonly PersonManagerContext _context;
        public PersonReadRepository(PersonManagerContext context)
        {
            _context = context;
        }

        public Person GetById(int id)
        {
            try
            {
                return _context.People.Find(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error getting person by id {id}");
                return null;
            }
        }

        public IEnumerable<Person> GetAll()
        {
            try
            {
                var test = _context.People.ToList();
                Log.Information($"Repository GetAll: {test.Count} people found");
                return test;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting all people");
                return Enumerable.Empty<Person>();
            }
        }
    }
}
