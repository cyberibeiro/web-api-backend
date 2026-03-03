using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_api.Repositories
{
    internal interface IRepository<T>
    {
        Task<List<T>> ReadAllAsync(); // indicamos que é um tarefa(task)
        Task<T> ReadAsync(int id);
        Task CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);

    }
}
