using Microsoft.EntityFrameworkCore;
using Tdd.Project.Domain.Interfaces.UnitOfWork;
using Tdd.Project.Infrastructure.Data;

namespace Tdd.Project.Infrastructure.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TarefaContext _context;

    public UnitOfWork(TarefaContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public int Commit()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}

