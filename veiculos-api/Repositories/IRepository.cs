using System.Collections.Generic;
using System.Threading.Tasks;

namespace veiculos_api.Repositories
{
    internal interface IRepository<T>
    {
        Task<List<T>> ReadAllAsync();
        Task<T> ReadAsync(int id);
        Task<List<T>> ReadAsync(string nome);
        Task CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
