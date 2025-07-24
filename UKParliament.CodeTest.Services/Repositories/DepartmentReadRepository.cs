using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;

namespace UKParliament.CodeTest.Services.Repositories
{
    public class DepartmentReadRepository : IDepartmentReadService<Department>
    {
        private readonly PersonManagerContext _context;
        private readonly ILogger<DepartmentReadRepository> _logger;
        public DepartmentReadRepository(PersonManagerContext context, ILogger<DepartmentReadRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Department? GetById(int id)
        {
            try
            {
                var department = _context.Departments.Find(id);
                if (department == null)
                {
                    // Return null if not found (nullable reference type)
                    return null;
                }
                return department;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting person by id {PersonId}", id);
                return null;
            }
        }

        public IEnumerable<Department> GetAll()
        {
            try
            {
                var test = _context.Departments.ToList();
                _logger.LogInformation("Repository GetAll: {Count} people found", test.Count);
                return test;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all people");
                return Enumerable.Empty<Department>();
            }
        }
    }
}

