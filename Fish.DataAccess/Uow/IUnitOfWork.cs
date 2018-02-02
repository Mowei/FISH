using System;
using System.Threading;
using System.Threading.Tasks;
using FISH.DataAccess.Entities;
using FISH.DataAccess.Repositories;

namespace FISH.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IRepository<TEntity> GetRepository<TEntity>();
        TRepository GetCustomRepository<TRepository>();
    }
}
