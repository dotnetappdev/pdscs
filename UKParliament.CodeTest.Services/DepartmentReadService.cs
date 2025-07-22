using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;

namespace UKParliament.CodeTest.Services
{
    public class DepartmentReadService : IDepartmentReadRepository
    {
        private readonly PersonManagerContext _context;
        public DepartmentReadService(PersonManagerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            // Replace with async EF call in real code
            var departments = await _context.Departments.ToListAsync();
            return departments;
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            // Replace with async EF call in real code
            var record = await _context.Departments.FindAsync(id);
            return record;

        }
    }
}
