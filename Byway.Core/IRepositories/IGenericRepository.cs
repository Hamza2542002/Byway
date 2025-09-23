using Byway.Core.Entities;
using System.Linq.Expressions;

namespace Byway.Core.IRepositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync(Func<IQueryable<T>,IQueryable<T>>? query = null);
    Task<int> GetCountAsync(Func<IQueryable<T>, IQueryable<T>>? query = null);
    Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? query = null);
    Task<T?> GetOneAsync(Expression<Func<T,bool>> predicate,Func<IQueryable<T>, IQueryable<T>>? query = null);
    Task AddAsync(T entity);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
}
