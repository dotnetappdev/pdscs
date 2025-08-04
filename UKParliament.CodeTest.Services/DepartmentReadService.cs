using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;
using Serilog;

namespace UKParliament.CodeTest.Services
{
    public class DepartmentReadService : IDepartmentReadService
    {
        private readonly PersonManagerContext _context;
        private readonly ILogger _logger;
        public DepartmentReadService(PersonManagerContext context, ILogger logger)
        {
            _context = context;
            _logger = logger.ForContext<DepartmentReadService>();
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                return departments;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all departments");
                throw;
            }
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            try
            {
                var record = await _context.Departments.FindAsync(id);
                return record; // Return null if not found instead of throwing
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting department by id {id}");
                throw;
            }
        }
    }
}
