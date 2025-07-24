using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Repositories
{
    public interface IDepartmentReadService<T>
    {
        Department? GetById(int id);
        IEnumerable<Department> GetAll();


    }
}
