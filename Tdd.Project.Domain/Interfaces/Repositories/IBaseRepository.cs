using System.Linq.Expressions;
using Tdd.Project.Domain.Entities.Base;

namespace Tdd.Project.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : EntityBase<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(int id);
    Task InsertAsync(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(int id);
    Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(int id);
}
