using System;
using System.Threading;
using System.Threading.Tasks;
using FISH.DataAccess.Entities;
using FISH.DataAccess.Repositories;

namespace FISH.DataAccess.Uow
{
    public interface IUnitOfWorkBase : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IRepository<TEntity> GetRepository<TEntity>();
        TRepository GetCustomRepository<TRepository>();
    }
}