namespace Tdd.Project.Domain.Entities.Base;

public abstract class EntityBase<T>
{
    public int Id { get; init; }
    public DateTime? DataCriacao { get; init; }

    protected EntityBase()
    {
        DataCriacao = DateTime.Now;
    }

    public virtual EntityBase<T> Update(T entity)
    {
        return this;
    }

}
