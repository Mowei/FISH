using System;
using FISH.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FISH.DataAccess.Uow
{
    public class UnitOfWork : UnitOfWorkBase<DbContext>, IUnitOfWork
    {
        public UnitOfWork(DbContext context, IServiceProvider provider) : base(context, provider)
        { }
    }
}
