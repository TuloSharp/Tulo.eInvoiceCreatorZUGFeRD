using Microsoft.EntityFrameworkCore;
using Tulo.Application.Interfaces.UnitOfWorks;

namespace Tulo.Infrastructure.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public UnitOfWorkFactory(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
       
    }

    public IUnitOfWork Create()
    {
        return new UnitOfWork(_dbFactory);
    }
}
