using System.Linq.Expressions;
using System.Xml;

namespace Infrastructuur.Services.Interfaces
{
    public interface IJsonDatabase<T>
    {
        Task<bool> CreateAsync(T item);
        Task<bool> UpdateAsync(Expression<Func<T, bool>> predicate, T updateTo);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate);
    }
}