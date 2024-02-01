namespace Tdd.Project.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();
    int Commit();
}
