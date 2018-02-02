using Microsoft.EntityFrameworkCore;

namespace FISH.DataAccess.Repositories
{
    public interface IRepositoryInjection
    {
        IRepositoryInjection SetContext(DbContext context);
    }
}