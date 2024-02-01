using TDD.Domain.Entities;

namespace Tdd.Project.Domain.Interfaces.Services;

public interface ITarefaService
{
    Task<bool> AdicionarTarefaAsync(Tarefa tarefa);
    Task<Tarefa> ObterTarefaPorId(int id);
    Task<IEnumerable<Tarefa>> ObterTodasAsTarefas();
    Task<bool> AtualizarTarefa(Tarefa tarefa);
    Task<bool> RemoverTarefa(int id);
}
