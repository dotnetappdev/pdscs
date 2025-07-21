using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKParliament.CodeTest.Services.Repositories
{
    public interface IWriteOnlyRepository<T>
    {
        void Save(T entity);
        void Delete(int id);
    }
}
