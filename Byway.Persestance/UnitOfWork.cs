using Byway.Core.Entities;
using Byway.Core.IRepositories;
using Byway.Persestance.Data;
using Byway.Persestance.Repositories;
using System.Collections;

namespace Byway.Persestance;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private Hashtable _repositories;
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _repositories = new();
    }
    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        var key = typeof(TEntity).Name;
        if (_repositories.ContainsKey(key))
        {
            return (IGenericRepository<TEntity>)_repositories[key];
        }
        var repo = new GenericRepository<TEntity>(_dbContext);

        _repositories.Add(key, repo);

        return repo;
    }
    public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();

    public async ValueTask DisposeAsync()
           => await _dbContext.DisposeAsync();
}
