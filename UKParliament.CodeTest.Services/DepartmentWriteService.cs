using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;

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
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            var existing = _context.Departments.Find(department.Id);
            if (existing == null) return null;
            existing.Name = department.Name;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null) return false;
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
