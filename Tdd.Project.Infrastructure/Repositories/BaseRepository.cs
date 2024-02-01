using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tdd.Project.Domain.Entities.Base;
using Tdd.Project.Domain.Interfaces.Repositories;

namespace Tdd.Project.Infrastructure.Repositories;

public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity> 
    where TEntity : EntityBase<TEntity>
    where TContext : DbContext
{ 
    private readonly TContext _context;
    private readonly ILogger<BaseRepository<TEntity, TContext>> _logger;

    public BaseRepository(TContext context, ILogger<BaseRepository<TEntity, TContext>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Delete(int id)
    {
        var entity = await GetByIdAsync(id);
        _context.Set<TEntity>().Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int item)
    {
        var result = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id!.Equals(item));
        return result!;
    }

    public async Task InsertAsync(TEntity entity)
    {
        var entityExists = await ExistsAsync(entity.Id!);
        if(entityExists)
        {
            throw new Exception("Registro já existente");
        }
        _context.Set<TEntity>().Add(entity);
    }

    public async Task Update(TEntity entity)
    {
        var entityExists = await ExistsAsync(entity.Id!);
        if(!entityExists)
        {
            throw new ArgumentException("Registro não existe.");
        }
        _context.Set<TEntity>().Update(entity);
    }

    public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = _context.Set<TEntity>().Where(predicate);
        return await result.ToListAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Set<TEntity>().AnyAsync(x => x.Id!.Equals(id));
    }

}

