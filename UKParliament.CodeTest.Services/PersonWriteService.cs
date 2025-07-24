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
        private readonly IPersonWriteService _writeRepository;
        private readonly ILogger _logger;
        public PersonWriteService(IPersonWriteService writeRepository, ILogger logger)
        {
            _writeRepository = writeRepository;
            _logger = logger.ForContext<PersonWriteService>();
        }

        public async void SavePerson(Person person)
        {
            try
            {
                await _writeRepository.AddPersonAsync(person);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error saving person");
                throw;
            }
        }

        public async void DeletePersonAsync(int id)
        {
            try
            {
                await _writeRepository.DeletePersonAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error deleting person with id {id}");
                throw;
            }
        }
    }
}
