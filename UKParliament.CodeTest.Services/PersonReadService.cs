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

    public class PersonReadService : IPersonReadService
    {
        private readonly IPersonReadService _readRepository;
        private readonly ILogger _logger;

        public PersonReadService(IPersonReadService readRepository, ILogger logger)
        {
            _readRepository = readRepository;
            _logger = logger.ForContext<PersonReadService>();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                return await _readRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting person by id {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            try
            {
                return await _readRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all persons");
                throw;
            }
        }
    }
}