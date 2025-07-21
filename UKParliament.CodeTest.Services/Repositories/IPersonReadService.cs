using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKParliament.CodeTest.Services.Repositories
{
    public interface IPersonReadService<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
