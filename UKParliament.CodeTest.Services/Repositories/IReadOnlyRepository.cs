using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKParliament.CodeTest.Services.Repositories
{
    public interface IReadOnlyRepository<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
