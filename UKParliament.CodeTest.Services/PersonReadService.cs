using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;
using Serilog;

namespace UKParliament.CodeTest.Services
{

    public class PersonReadService : IPersonReadService<Person>
    {
        private readonly IPersonReadService<Person> _readRepository;
        private readonly ILogger _logger;

        public PersonReadService(IPersonReadService<Person> readRepository, ILogger logger)
        {
            _readRepository = readRepository;
            _logger = logger.ForContext<PersonReadService>();
        }

        public Person GetById(int id)
        {
            try
            {
                return _readRepository.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting person by id {id}");
                throw;
            }
        }

        public IEnumerable<Person> GetAll()
        {
            try
            {
                return _readRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all persons");
                throw;
            }
        }
    }
}