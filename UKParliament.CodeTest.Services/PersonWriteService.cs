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
    public class PersonWriteService
    {
        private readonly IPersonWriteService<Person> _writeRepository;
        private readonly ILogger _logger;
        public PersonWriteService(IPersonWriteService<Person> writeRepository, ILogger logger)
        {
            _writeRepository = writeRepository;
            _logger = logger.ForContext<PersonWriteService>();
        }

        public void SavePerson(Person person)
        {
            try
            {
                _writeRepository.SavePerson(person);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error saving person");
                throw;
            }
        }

        public void DeletePerson(int id)
        {
            try
            {
                _writeRepository.DeletePerson(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error deleting person with id {id}");
                throw;
            }
        }
    }
}
