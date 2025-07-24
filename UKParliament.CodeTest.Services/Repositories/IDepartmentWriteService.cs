using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web;

namespace UKParliament.CodeTest.Services.Repositories
{
    public interface IDepartmentWriteService<T>
    {
        void SaveDepartment(Department dept);
        void Delete(int id);
        ApiResponse<Department> AddDepartment(Department department);
        ApiResponse<Department> UpdateDepartment(int id, Department updatedDepartment);

        Task<ApiResponse<Department>> DeleteDepartment(int id);




    }
}
