using Byway.Core.Entities;

namespace Byway.Core.IRepositories;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

    Task<int> CompleteAsync();
}
