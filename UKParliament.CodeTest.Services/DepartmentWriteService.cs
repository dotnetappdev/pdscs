using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;
using Serilog;

namespace UKParliament.CodeTest.Services
{
    public class DepartmentWriteService : IDepartmentWriteRepository
    {
        private readonly PersonManagerContext _context;
        public DepartmentWriteService(PersonManagerContext context)
        {
            _context = context;
        }

        public async Task<Department> AddAsync(Department department)
        {
            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return department;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding department");
                throw;
            }
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            try
            {
                var existing = _context.Departments.Find(department.Id);
                if (existing == null) return null;
                existing.Name = department.Name;
                await _context.SaveChangesAsync();
                return existing;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error updating department with id {department.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var department = _context.Departments.Find(id);
                if (department == null) return false;
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error deleting department with id {id}");
                throw;
            }
        }
    }
}
