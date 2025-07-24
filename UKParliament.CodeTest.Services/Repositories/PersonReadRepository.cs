using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UKParliament.CodeTest.Services.Repositories;

namespace UKParliament.CodeTest.Data.Repositories
{
    public class PersonReadRepository : IPersonReadService
    {
        private readonly PersonManagerContext _context;
        private readonly ILogger<PersonReadRepository> _logger;
        public PersonReadRepository(PersonManagerContext context, ILogger<PersonReadRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                var person = await _context.People.FindAsync(id);
                if (person == null)
                {
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

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            try
            {
                var test = await _context.People.ToListAsync();
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
