using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posts_api.Interfaces
{
    internal interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task CreateAsync(T post);
        Task<bool> UpdateAsync(T post);
        Task<bool> DeleteAsync(int id);
    }
}
