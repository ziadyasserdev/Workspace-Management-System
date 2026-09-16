using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Contracts.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        IQueryable<T> Query();
    }
}
