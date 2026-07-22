using Microsoft.EntityFrameworkCore;
using Tulo.Application.Interfaces.UnitOfWorks;

namespace Tulo.Infrastructure.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<AppDbContext> dbFactory) : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory = dbFactory;

    public IUnitOfWork Create()
    {
        return new UnitOfWork(_dbFactory);
    }
}
