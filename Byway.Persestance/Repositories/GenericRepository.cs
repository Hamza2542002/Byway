using Byway.Core.Entities;
using Byway.Core.IRepositories;
using Byway.Persestance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Byway.Persestance.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _dbContext;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IReadOnlyList<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? query = null)
    {
        var dbSet = _dbContext.Set<T>().AsQueryable();
        if (query is not null)
        {
            dbSet = query(dbSet);
        }
        return await dbSet.ToListAsync();
    }

    public async Task<int> GetCountAsync(Func<IQueryable<T>, IQueryable<T>>? query = null)
    {
        var dbSet = _dbContext.Set<T>().AsQueryable();
        if (query is not null)
        {
            dbSet = query(dbSet);
        }
        return await dbSet.CountAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? query = null)
    {
        var dbSet = _dbContext.Set<T>().Where(e => e.Id == id).AsQueryable();
        if (query is not null)
        {
            dbSet = query(dbSet);
        }
        return await dbSet.FirstOrDefaultAsync();
    }
    public async Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? query = null)
    {
        var dbSet = _dbContext.Set<T>().Where(predicate).AsQueryable();
        if (query is not null)
        {
            dbSet = query(dbSet);
        }
        return await dbSet.FirstOrDefaultAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
    }

    public void DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

}
