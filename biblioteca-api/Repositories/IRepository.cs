using System.Collections.Generic;
using System.Threading.Tasks;

namespace biblioteca_api.Repositories
{
    internal interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
