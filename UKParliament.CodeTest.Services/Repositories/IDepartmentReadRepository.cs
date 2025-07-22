using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Repositories
{
    public interface IDepartmentReadRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
    }
}
