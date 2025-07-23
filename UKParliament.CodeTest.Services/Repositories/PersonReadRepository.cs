using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UKParliament.CodeTest.Services.Repositories;

namespace UKParliament.CodeTest.Data.Repositories
{
    public class PersonReadRepository : IPersonReadService<Person>
    {
        private readonly PersonManagerContext _context;
        private readonly ILogger<PersonReadRepository> _logger;
        public PersonReadRepository(PersonManagerContext context, ILogger<PersonReadRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Person? GetById(int id)
        {
            try
            {
                var person = _context.People.Find(id);
                if (person == null)
                {
                    // Return null if not found (nullable reference type)
                    return null;
                }
                return person;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting person by id {PersonId}", id);
                return null;
            }
        }

        public IEnumerable<Person> GetAll()
        {
            try
            {
                var test = _context.People.ToList();
                _logger.LogInformation("Repository GetAll: {Count} people found", test.Count);
                return test;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all people");
                return Enumerable.Empty<Person>();
            }
        }
    }
}
