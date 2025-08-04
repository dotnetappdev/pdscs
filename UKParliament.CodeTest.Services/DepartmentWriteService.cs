using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;
using Microsoft.Extensions.Logging;

namespace UKParliament.CodeTest.Services
{
    public class DepartmentWriteService : IDepartmentWriteService
    {
        private readonly PersonManagerContext _context;
        private readonly ILogger<DepartmentWriteService> _logger;
        public DepartmentWriteService(PersonManagerContext context, ILogger<DepartmentWriteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Department?> AddAsync(Department department)
        {
            try
            {
                if (department == null || string.IsNullOrWhiteSpace(department.Name))
                {
                    // Return null if input is null or Name is null/empty
                    return null;
                }
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return department;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding department");
                throw;
            }
        }

        public async Task<Department?> UpdateAsync(Department department)
        {
            try
            {
                if (department == null)
                {
                    // Return null if input is null
                    return null;
                }
                var existing = _context.Departments.Find(department.Id);
                if (existing == null)
                {
                    // Explicitly return null if not found (nullable reference type)
                    return null;
                }
                existing.Name = department.Name;
                await _context.SaveChangesAsync();
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department with id {DepartmentId}", department?.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var department = _context.Departments.Find(id);
                if (department == null)
                {
                    // Return false if not found, no null reference risk
                    return false;
                }
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department with id {DepartmentId}", id);
                throw;
            }
        }
    }
}
