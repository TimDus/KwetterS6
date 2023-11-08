using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Create(T obj);
        Task<T> GetById(object id);
        Task Update(T obj);
        Task Delete(object id);
    }
}
